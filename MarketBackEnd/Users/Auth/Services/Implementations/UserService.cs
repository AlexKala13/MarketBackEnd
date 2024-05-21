using MarketBackEnd.Users.Auth.Services.Interfaces;

namespace MarketBackEnd.Users.Auth.Services.Implementations
{
    public class UserService : IUserService
    {
        public bool IsAdmin(int userId)
        {
            return userId == 0;
        }

        public bool IsAuthor(int userId, int advertisementUserId)
        {
            return userId == advertisementUserId;
        }
    }
}
