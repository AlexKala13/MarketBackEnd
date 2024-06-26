﻿using MarketBackEnd.Shared.Model;
using MarketBackEnd.Users.Auth.DTOs;
using MarketBackEnd.Users.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketBackEnd.Users.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDTO request)
        {
            var response = await _authRepository.Register(new User
            {
                Email = request.Email,
                UserName = request.UserName,
                Address = request.Address,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Telephone = request.Telephone
            }, request.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDTO request)
        {
            var response = await _authRepository.Login(request.Email, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<ServiceResponse<string>>> ForgotPassword(ForgotPasswordDTO request)
        {
            var response = await _authRepository.ForgotPassword(request.Email);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("ResetPassword")]
        public async Task<ActionResult<ServiceResponse<string>>> ResetPassword(ResetPasswordDTO request)
        {
            var response = await _authRepository.ResetPassword(request.Email, request.Token, request.NewPassword);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
