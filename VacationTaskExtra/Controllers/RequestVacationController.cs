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
        public async Task<IActionResult> Index()
        {
            var vacationDbContext = context.RequestVacations.Include(r => r.VacationType);
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
            ViewData["FK_VacationType"] = new SelectList(context.VacationTypes, "TypeId", "TypeId");
            return View();
        }

        // POST: RequestVacation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("RequestVacId,DateStart,DateEnd,FK_VacationType,FK_Personel")] RequestVacationModel requestVacationModel)
        {
            //var user = await userManager.GetUserAsync(HttpContext.User);
                string currentUserId = userManager.GetUserId(HttpContext.User);
                requestVacationModel.FK_Personel = currentUserId;

                context.Add(requestVacationModel);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        
            ViewData["FK_VacationType"] = new SelectList(context.VacationTypes, "TypeId", "TypeId", requestVacationModel.FK_VacationType);
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
            ViewData["FK_VacationType"] = new SelectList(context.VacationTypes, "TypeId", "TypeId", requestVacationModel.FK_VacationType);
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

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(requestVacationModel);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestVacationModelExists(requestVacationModel.RequestVacId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FK_VacationType"] = new SelectList(context.VacationTypes, "TypeId", "TypeId", requestVacationModel.FK_VacationType);
            return View(requestVacationModel);
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

        private bool RequestVacationModelExists(int id)
        {
          return (context.RequestVacations?.Any(e => e.RequestVacId == id)).GetValueOrDefault();
        }

    }
}
