using MarketBackEnd.Model;
using Microsoft.EntityFrameworkCore;

namespace MarketBackEnd.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Advertisement> Advertisements => Set<Advertisement>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Photos> Photos => Set<Photos>();
    }
}
