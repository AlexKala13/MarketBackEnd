﻿using Azure.Core;
using MarketBackEnd.Products.Advertisements.DTOs.Advertisement;
using MarketBackEnd.Shared.Model;
using MarketBackEnd.Users.Auth.Services.Interfaces;
using MarketBackEnd.Users.Customer.DTOs;
using MarketBackEnd.Users.Customer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketBackEnd.Users.Customer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserInfoDTO>>> GetSingle(int id)
        {
            return await _userService.GetUserInfo(id);
        }

        [HttpPut("Edit/{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateUser(UserEditDTO editedUser, int id)
        {
            var response = await _userService.EditUser(editedUser, id);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
