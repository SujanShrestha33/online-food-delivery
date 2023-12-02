using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineFoodOrdering.Models.Entity;
namespace OnlineFoodOrdering.Areas.Identity.Data
{
    public class AppIdentityContext : IdentityDbContext<ApplicationUsers>
    {
        public AppIdentityContext(DbContextOptions<AppIdentityContext> options) : base(options) { }

        protected AppIdentityContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
}
