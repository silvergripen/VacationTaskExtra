using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VacationTaskExtra.Data;
using VacationTaskExtra.Models;
using Microsoft.AspNetCore.Identity; // Add this namespace for UserManager
using Microsoft.AspNetCore.Http; // Add this namespace for IHttpContextAccessor

[Authorize] // Ensure that only authenticated users can access this controller
public class TimeLeftController : Controller
{
    private readonly VacationDbContext context;
    private readonly UserManager<PersonelModel> userManager;
    private readonly IHttpContextAccessor httpContextAccessor;

    public TimeLeftController(VacationDbContext context, UserManager<PersonelModel> userManager, IHttpContextAccessor httpContextAccessor)
    {
        this.context = context;
        this.userManager = userManager;
        this.httpContextAccessor = httpContextAccessor;
    }

    // GET: TimeLeft
    public async Task<IActionResult> Index()
    {
        // Retrieve the currently logged-in user
        var user = await userManager.GetUserAsync(HttpContext.User);

        if (user == null)
        {
            return NotFound(); // Handle the case where the user is not found
        }

        // Retrieve all time left records for the logged-in personnel, including the VacationType
        var timeLeftRecords = await context.TimeLefts
            .Where(t => t.FK_Personel == user.Id)
            .Include(t => t.VacationType) // Include the VacationType
            .ToListAsync();

        return View(timeLeftRecords); // Pass the list of records to the view
    }
}