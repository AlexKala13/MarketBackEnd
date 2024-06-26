﻿using MarketBackEnd.Shared.Model;
using MarketBackEnd.Users.Customer.DTOs;

namespace MarketBackEnd.Users.Customer.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<GetUserInfoDTO>> GetUserInfo(int id);
        Task<ServiceResponse<User>> EditUser(UserEditDTO user, int id);
        Task<bool> IsAdmin(int userId);
        bool IsAuthor(int userId, int advertisementUserId);
    }
}
