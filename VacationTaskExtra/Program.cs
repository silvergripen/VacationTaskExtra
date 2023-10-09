using VacationTaskExtra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using VacationTaskExtra.Models;
using VacationTaskExtra.Controllers;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<VacationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
.LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddDefaultIdentity<PersonelModel>(options =>
options.SignIn.RequireConfirmedAccount = true)
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<VacationDbContext>();


builder.Services.AddRazorPages();

//builder.Services.AddIdentity<PersonelModel, IdentityRole>()
//    .AddEntityFrameworkStores<VacationDbContext>()
//    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}
//using (var scope = app.Services.CreateScope())
//{ 
//    //vill inte fungera
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<
//
//    >>();
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


//    string email = "admin@admin.com";
//    string password = "password";

//    var user = new IdentityUser();
//    user.UserName = email;
//    user.Email = email;
//    user.EmailConfirmed = true;

//    var result = await userManager.CreateAsync(user, password);

//    if (result.Succeeded)
//    {
//        await userManager.AddToRoleAsync(user, "Admin");
//    }
//    else
//    {
//        foreach (var error in result.Errors)
//        {
//            Console.WriteLine($"Error: {error.Description}");
//        }
//    }

//}
app.Run();
