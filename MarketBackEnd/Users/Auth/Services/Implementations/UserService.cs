using MarketBackEnd.Shared.Data;
using MarketBackEnd.Users.Auth.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketBackEnd.Users.Auth.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;

        public UserService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> IsAdmin(int userId)
        {
            var user = await _db.Users.FindAsync(userId);
            return user != null && user.IsAdmin;
        }

        public bool IsAuthor(int userId, int advertisementUserId)
        {
            return userId == advertisementUserId;
        }
    }
}
