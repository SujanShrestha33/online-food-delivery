using Microsoft.EntityFrameworkCore;
using OnlineFoodOrdering.Models.Entity;

namespace OnlineFoodOrdering.Models.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Food> Foods { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<SubCategories> SubCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define the primary key for Food entity
            modelBuilder.Entity<Food>()
                .HasKey(f => f.FoodId);

            // Define the foreign key relationship between Food and SubCategories
            modelBuilder.Entity<Food>()
                .HasOne(f => f.SubCategory)
                .WithMany(s => s.Foods)
                .HasForeignKey(f => f.SubCategoryId);

            // Define the primary key for Categories entity
            modelBuilder.Entity<Categories>()
                .HasKey(c => c.CategoryId);

            // Define the relationship between Categories and SubCategories
            modelBuilder.Entity<Categories>()
                .HasMany(c => c.SubCategories)
                .WithOne(s => s.Category)
                .HasForeignKey(s => s.CategoryId);

            // Define the primary key for SubCategories entity
            modelBuilder.Entity<SubCategories>()
                .HasKey(s => s.SubCategoryId);

            // Add any other configurations or constraints as needed

            base.OnModelCreating(modelBuilder);
        }
        }
}
