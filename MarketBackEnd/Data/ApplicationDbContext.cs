using MarketBackEnd.Model;
using Microsoft.EntityFrameworkCore;

namespace MarketBackEnd.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<User> users => Set<User>();
    }
}
