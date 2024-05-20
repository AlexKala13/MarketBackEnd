using MarketBackEnd.Products.Advertisements.Models;
using Microsoft.EntityFrameworkCore;
using MarketBackEnd.Users.Auth.Models;

namespace MarketBackEnd.Shared.Data
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
