namespace MarketBackEnd.Users.Auth.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> IsAdmin(int userId);
        bool IsAuthor(int userId, int advertisementUserId);
    }
}
