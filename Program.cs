using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentElectionSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// =============================
// DATABASE
// =============================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36))
    ));

// =============================
// IDENTITY + ROLES
// =============================
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// =============================
// RAZOR + SESSION + HTTPCLIENT
// =============================
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddHttpClient(); // <-- Fix for IHttpClientFactory injection

// =============================
// BUILD APP
// =============================
var app = builder.Build();

// =============================
// MIDDLEWARE
// =============================
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

// =============================
// SEED ROLES + DEFAULT USERS
// =============================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = { "Student", "Admin", "Supervisor" };

    // Create Roles
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // =============================
    // CREATE DEFAULT ADMIN
    // =============================
    var adminEmail = "admin@system.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(adminUser, "Admin@123");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    // =============================
    // CREATE DEFAULT SUPERVISOR
    // =============================
    var supervisorEmail = "supervisor@system.com";
    var supervisorUser = await userManager.FindByEmailAsync(supervisorEmail);

    if (supervisorUser == null)
    {
        supervisorUser = new IdentityUser
        {
            UserName = supervisorEmail,
            Email = supervisorEmail,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(supervisorUser, "Supervisor@123");
        await userManager.AddToRoleAsync(supervisorUser, "Supervisor");
    }
}

app.Run();
