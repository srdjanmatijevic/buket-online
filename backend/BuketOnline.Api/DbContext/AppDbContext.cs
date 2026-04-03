using Microsoft.EntityFrameworkCore;
using BuketOnline.Api.Models;

namespace BuketOnline.Api
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Flower> Flowers { get; set; }
        public DbSet<FlowerCategory> FlowerCategories { get; set; }
        public DbSet<Bouquet> Bouquets { get; set; }
        public DbSet<BouquetItem> BouquetItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flower>()
                .HasOne(f => f.FlowerCategory)
                .WithMany(c => c.Flowers)
                .HasForeignKey(f => f.FlowerCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BouquetItem>()
                .HasOne(bi => bi.Bouquet)
                .WithMany(b => b.Items)
                .HasForeignKey(bi => bi.BouquetId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BouquetItem>()
                .HasOne(bi => bi.Flower)
                .WithMany()
                .HasForeignKey(bi => bi.FlowerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}