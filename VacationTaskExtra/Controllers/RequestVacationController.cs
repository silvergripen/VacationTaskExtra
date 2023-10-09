using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VacationTaskExtra.Data;
using VacationTaskExtra.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace VacationTaskExtra.Controllers
{
    public class RequestVacationController : Controller
    {
        private readonly UserManager<PersonelModel> userManager;
        private readonly VacationDbContext context;
        private IHttpContextAccessor httpContextAccessor;

        public RequestVacationController(VacationDbContext context, UserManager<PersonelModel> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        // GET: RequestVacation
        public async Task<IActionResult> Index(bool showAll = false, bool showPending = false, bool showAccepted = false, bool showRejected = false, string searchString = "")
        {

            IQueryable<RequestVacationModel> vacationDbContext = context.RequestVacations
            .Include(r => r.VacationType)
            .Include(w => w.WaitingRequest)
            .Include(p => p.Personels);
            var user = await userManager.GetUserAsync(HttpContext.User);
            bool isAdmin = await userManager.IsInRoleAsync(user, "Admin");

            // Check if the user is an admin, and adjust the filters accordingly
            if (isAdmin)
            {
                // Administrator can see all vacation requests
                showAll = true;
            }
            else
            {
                // Regular user can only see their own requests
                showAll = false;
            }
            if (!showAll)
            {
                // Apply a filter to show only vacations that haven't ended yet
                vacationDbContext = vacationDbContext.Where(v => v.DateEnd >= DateTime.Now);
            }

            if (showPending)
            {
                // Apply a filter to show only vacations with "Pending" status
                vacationDbContext = vacationDbContext.Where(v => v.WaitingRequest.AcceptReject == "Pending");
            }

            if (showAccepted)
            {
                // Apply a filter to show only vacations with "Accepted" status
                vacationDbContext = vacationDbContext.Where(v => v.WaitingRequest.AcceptReject == "Accepted");
            }

            if (showRejected)
            {
                // Apply a filter to show only vacations with "Rejected" status
                vacationDbContext = vacationDbContext.Where(v => v.WaitingRequest.AcceptReject == "Rejected");
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                // Apply a filter to search for users by FullName
                vacationDbContext = vacationDbContext.Where(v => v.Personels.FullName.Contains(searchString));
            }

            ViewData["SearchString"] = searchString;

            // Create a list of SelectListItem for the WaitingRequestModels
            var waitingRequestModels = await context.WaitingRequests.ToListAsync();
            var waitingRequestSelectList = new SelectList(waitingRequestModels, "CurrentVacId", "AcceptReject");

            // Set the SelectList in ViewData
            ViewData["WaitingRequestModels"] = waitingRequestSelectList;

            return View(await vacationDbContext.ToListAsync());
        }


        // GET: RequestVacation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || context.RequestVacations == null)
            {
                return NotFound();
            }

            var requestVacationModel = await context.RequestVacations
                .Include(r => r.VacationType)
                .FirstOrDefaultAsync(m => m.RequestVacId == id);
            if (requestVacationModel == null)
            {
                return NotFound();
            }

            return View(requestVacationModel);
        }

        // GET: RequestVacation/Create
        public IActionResult Create()
        {
            ViewData["FK_VacationType"] = new SelectList(context.VacationTypes, "TypeId", "TypeName");
            return View();
        }

        // POST: RequestVacation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestVacId,DateStart,DateEnd,FK_VacationType,FK_Personel")] RequestVacationModel requestVacationModel)
        {
            string currentUserId = userManager.GetUserId(HttpContext.User);
            requestVacationModel.FK_Personel = currentUserId;
            requestVacationModel.FK_WaitingRequestModel = 1;

            // Calculate the duration of the vacation in days
            int duration = (int)(requestVacationModel.DateEnd - requestVacationModel.DateStart).TotalDays;

            // Get the user's remaining time for the selected vacation type
            var timeLeft = await context.TimeLefts
                .Where(t => t.FK_Personel == currentUserId && t.FK_VacationType == requestVacationModel.FK_VacationType)
                .FirstOrDefaultAsync();

            if (timeLeft != null)
            {
                // Check if the user has enough remaining time for the vacation
                if (timeLeft.TimeLeft >= duration)
                {
                    // Deduct the vacation duration from the remaining time
                    timeLeft.TimeLeft -= duration;
                    context.Update(timeLeft);

                    // Add the requestVacationModel to the context
                    context.Add(requestVacationModel);

                    // Save the changes to the database
                    await context.SaveChangesAsync();

                    ViewData["FK_VacationType"] = new SelectList(context.VacationTypes, "TypeId", "TypeName", requestVacationModel.FK_VacationType);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Not enough remaining time for this vacation type.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "No remaining time for this vacation type.");
            }

            ViewData["FK_VacationType"] = new SelectList(context.VacationTypes, "TypeId", "TypeName", requestVacationModel.FK_VacationType);
            return View(requestVacationModel);
        }

        // GET: RequestVacation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || context.RequestVacations == null)
            {
                return NotFound();
            }

            var requestVacationModel = await context.RequestVacations.FindAsync(id);
            if (requestVacationModel == null)
            {
                return NotFound();
            }
            ViewData["FK_VacationType"] = new SelectList(context.VacationTypes, "TypeId", "TypeName", requestVacationModel.FK_VacationType);
            return View(requestVacationModel);
        }

        // POST: RequestVacation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestVacId,DateStart,DateEnd,FK_VacationType,FK_Personel")] RequestVacationModel requestVacationModel)
        {
            if (id != requestVacationModel.RequestVacId)
            {
                return NotFound();
            }


            context.Update(requestVacationModel);
            await context.SaveChangesAsync();

               
            ViewData["FK_VacationType"] = new SelectList(context.VacationTypes, "TypeId", "TypeName", requestVacationModel.FK_VacationType);
            return RedirectToAction(nameof(Index));
        }

        // GET: RequestVacation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || context.RequestVacations == null)
            {
                return NotFound();
            }

            var requestVacationModel = await context.RequestVacations
                .Include(r => r.VacationType)
                .FirstOrDefaultAsync(m => m.RequestVacId == id);
            if (requestVacationModel == null)
            {
                return NotFound();
            }

            return View(requestVacationModel);
        }

        // POST: RequestVacation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (context.RequestVacations == null)
            {
                return Problem("Entity set 'VacationDbContext.RequestVacations'  is null.");
            }
            var requestVacationModel = await context.RequestVacations.FindAsync(id);
            if (requestVacationModel != null)
            {
                context.RequestVacations.Remove(requestVacationModel);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //THIS DOES NOT WORK int newWaitingRequestId is 0 and it cant be that value. Meaning it gets no value from the index view.
        [HttpPost]
        public async Task<IActionResult> UpdateWaitingRequest(int id, int newWaitingRequestId)
        {
            // Retrieve the RequestVacationModel by id
            var requestVacationModel = await context.RequestVacations.FindAsync(id);

            if (requestVacationModel != null)
            {
                // Update the FK_WaitingRequestModel with the new value
                requestVacationModel.FK_WaitingRequestModel = newWaitingRequestId;
                context.Update(requestVacationModel);
                await context.SaveChangesAsync();
            }

            // Redirect back to the Index action or another appropriate action
            return RedirectToAction(nameof(Index));
        }
        private bool IsActive(int id)
        {
            //Check if current date is same as datestart and is accpeted then set active. 
            //While active lower max time.
            return(true);
        }
        private bool RequestVacationModelExists(int id)
        {
          return (context.RequestVacations?.Any(e => e.RequestVacId == id)).GetValueOrDefault();
        }

    }
}
