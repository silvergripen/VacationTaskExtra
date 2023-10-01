using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VacationTaskExtra.Data;

namespace VacationTaskExtra.Controllers
{
    public class PersonelController : Controller
    {

        private readonly UserManager<IdentityUser> userManager;
        private readonly VacationDbContext context;


        public PersonelController(VacationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || context.RequestVacations == null)
            {
                return NotFound();
            }

            var requestVacationModel = await context.Personels.FindAsync(id);
            if (requestVacationModel == null)
            {
                return NotFound();
            }
            //ViewData["FK_VacationType"] = new SelectList(context.Roles, "TypeId", "TypeName", requestVacationModel.FK_VacationType);
            //ViewData["FK_WaitingRequest"] = new SelectList(context.WaitingRequests, "CurrentVacId", "AcceptReject", requestVacationModel.WaitingRequest);
            return View(requestVacationModel);
        }
    }
}
