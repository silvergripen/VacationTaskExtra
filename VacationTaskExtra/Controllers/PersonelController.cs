using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VacationTaskExtra.Data;
using VacationTaskExtra.Models;

namespace VacationTaskExtra.Controllers
{
    public class PersonelController : Controller
    {

        private readonly UserManager<PersonelModel> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly VacationDbContext context;


        public PersonelController(VacationDbContext context, UserManager<PersonelModel> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var usersWithRoles = await userManager.Users
                .Select(user => new
                {
                    user.Id,
                    user.UserName,
                    user.FullName,
                    Roles = userManager.GetRolesAsync(user).Result
                })
                .ToListAsync();

            return View(usersWithRoles);
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personelModel = await context.Personels.FindAsync(id);

            if (personelModel == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(context.Personels, "Id", "Fullname");
            return View(personelModel);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var personelModel = await context.Personels.FindAsync(id);

            if (personelModel == null)
            {
                return NotFound();
            }

            context.Personels.Remove(personelModel);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirect to a list of all Personels or another appropriate page.
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personelModel = await context.Personels.FindAsync(id);

            if (personelModel == null)
            {
                return NotFound();
            }

            // Retrieve all roles
            var allRoles = roleManager.Roles.ToList();

            // Retrieve the roles for the user
            var user = await userManager.FindByIdAsync(id);
            var userRoles = await userManager.GetRolesAsync(user);

            // Create a list of SelectListItem for roles, setting the selected role for the user
            var roleList = new List<SelectListItem>();
            foreach (var role in allRoles)
            {
                roleList.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Name,
                    Selected = userRoles.Contains(role.Name)
                });
            }

            ViewBag.Roles = roleList;

            return View(personelModel);
        }

        public async Task<IActionResult> Edit(int id, [Bind("")] RequestVacationModel requestVacationModel)
        {
            return View();
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return View();
        }
     }
}
