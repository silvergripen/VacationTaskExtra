using VacationTaskExtra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacationTaskExtra.Models;

namespace VacationTaskExtra.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<PersonelModel> userManager;
        private readonly VacationDbContext context;
        public AdminController(VacationDbContext context, UserManager<PersonelModel> userManager)
        {
            this.context = context;
            this.userManager = userManager;

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetPersonel(string search = "")
        {
            string úserId = userManager.GetUserId(User);
            search = search.Trim();
            search = search.ToLower();
            if (search == "")
            {
                var getPersonel = await context.Personels
                    .Include(p => p.Email)
                    .Include(d => d.FullName)
                    .ToListAsync();
                return View("SearchPersonel", getPersonel);
            }
            else
            {
                var getPersonel = await context.Personels
                    .Include(p => p.Email)
                    .Include(o => o.FullName)
                    .Where(d => d.Email.ToLower().Contains(search) ||
                     d.FullName.ToLower().Contains(search)
                     ).ToListAsync();
                return View("SearchPersonel", getPersonel);
            }
        }
        public async Task<IActionResult> GetVacations(string search = "")
        {
            var getVacation = await context.VacationStatuses
            .Include(p => p.Personel)
                .ThenInclude(cr => cr.RequestVacations!)
                    .ThenInclude(rv => rv.VacationType)
            .Include(p => p.WaitingRequest!) // Use ! to handle nullability
            .Where(r => r.Personel.FullName.ToLower().Contains(search) ||
                         r.Personel.Email.ToLower().Contains(search))
            .ToListAsync();
            return View("SearchVacation", getVacation);
        }
        //Edit behöver göra en till edit och delete men kom ihåg post och validate på dom 
        //Endast på de som kommer posta.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVacations()
        {
            return View();
        }
        //Edit
        public async Task<IActionResult> DeleteVacations()
        {
            return View();
        }
        public async Task<IActionResult> AcceptDenyRequests()
        {
            return View();
        }
    }
}