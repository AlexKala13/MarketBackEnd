using MarketBackEnd.Shared.Data;
using MarketBackEnd.Shared.Model;
using MarketBackEnd.Users.Auth.Services.Implementations;
using MarketBackEnd.Users.Auth.Services.Interfaces;
using MarketBackEnd.Users.Customer.DTOs;
using MarketBackEnd.Users.Customer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketBackEnd.Users.Customer.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthRepository _authRepository;

        public UserService(ApplicationDbContext db, IAuthRepository authRepository)
        {
            _db = db;
            _authRepository = authRepository;
        }

        public async Task<ServiceResponse<User>> EditUser(UserEditDTO updatedUser, string password, int id)
        {
            var serviceResponse = new ServiceResponse<User>();
            try
            {
                var User = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (User == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "User not found.";
                    return serviceResponse;
                }

                if (User.Email != updatedUser.Email && await _authRepository.UserExists(updatedUser.Email))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "This email is already used.";
                    return serviceResponse;
                }

                User.Email = updatedUser.Email ?? User.Email;
                User.UserName = updatedUser.UserName ?? User.UserName;
                User.FirstName = updatedUser.FirstName ?? User.FirstName;
                User.LastName = updatedUser.LastName ?? User.LastName;
                User.Address = updatedUser.Address ?? User.Address;
                User.Telephone = updatedUser.Telephone ?? User.Telephone;
                User.IsActive = updatedUser.IsActive ?? true;

                if (!string.IsNullOrEmpty(password))
                {
                    _authRepository.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                    User.PasswordHash = passwordHash;
                    User.PasswordSalt = passwordSalt;
                }

                _db.Users.Update(User);
                await _db.SaveChangesAsync();

                serviceResponse.Data = User;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Failed to update user: " + ex.Message;
            }

            return serviceResponse;
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
