using Microsoft.EntityFrameworkCore;
using BuketOnline.Api.Models;

namespace BuketOnline.Api
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Flower> Flowers { get; set; }
        public DbSet<FlowerCategory> FlowerCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flower>()
                .HasOne(f => f.FlowerCategory)
                .WithMany(c => c.Flowers)
                .HasForeignKey(f => f.FlowerCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}