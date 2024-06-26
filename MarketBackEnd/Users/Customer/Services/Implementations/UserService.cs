﻿using AutoMapper;
using MarketBackEnd.Products.Advertisements.DTOs.Advertisement;
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
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext db, IAuthRepository authRepository, IMapper mapper)
        {
            _db = db;
            _authRepository = authRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<User>> EditUser(UserEditDTO updatedUser, int id)
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

                if (!string.IsNullOrEmpty(updatedUser.NewPassword))
                {
                    if(!_authRepository.VerifyPasswordHash(updatedUser.OldPassword, User.PasswordHash, User.PasswordSalt))
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = "Your password is incorrect.";
                        return serviceResponse;
                    }
                    _authRepository.CreatePasswordHash(updatedUser.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
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

        public async Task<ServiceResponse<GetUserInfoDTO>> GetUserInfo(int id)
        {
            var serviceResponse = new ServiceResponse<GetUserInfoDTO>();
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (user != null)
                {
                    var userInfoDTO = _mapper.Map<GetUserInfoDTO>(user);
                    serviceResponse.Data = userInfoDTO;
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "User not found.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found " + ex.Message;
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
