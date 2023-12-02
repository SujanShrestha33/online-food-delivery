using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineFoodOrdering.Areas.Identity.Data;
using OnlineFoodOrdering.Models.Data;
using Microsoft.AspNetCore.Identity;
using OnlineFoodOrdering.Models.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AppIdentityContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUsers>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;

    // Other Identity options...
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppIdentityContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var identityContext = scope.ServiceProvider.GetRequiredService<AppIdentityContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUsers>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    dbContext.Database.Migrate();
    identityContext.Database.Migrate();

    if (!roleManager.RoleExistsAsync("Admin").Result)
    {
        var role = new IdentityRole { Name = "Admin" };
        var result = roleManager.CreateAsync(role).Result;
    }

    var adminUser = userManager.FindByEmailAsync("admin@example.com").Result;
    if (adminUser == null)
    {
        adminUser = new ApplicationUsers
        {
            UserName = "admin",
            Email = "admin@example.com",
            // Add any additional properties for the admin user here
        };
        var result = userManager.CreateAsync(adminUser, "Admin@123").Result;

        if (result.Succeeded)
        {
            var roleResult = userManager.AddToRoleAsync(adminUser, "Admin").Result;
        }
    }
}

app.Run();
