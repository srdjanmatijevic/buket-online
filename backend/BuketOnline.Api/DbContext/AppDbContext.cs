using Microsoft.EntityFrameworkCore;
using BuketOnline.Api.Models;

namespace BuketOnline.Api
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Flower> Flowers { get; set; }
    }
}