using MarketBackEnd.Shared.Model;
using MarketBackEnd.Users.Auth.Models;

namespace MarketBackEnd.Users.Auth.Services.Interfaces
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string email, string password);
        Task<bool> UserExists(string email);
    }
}
