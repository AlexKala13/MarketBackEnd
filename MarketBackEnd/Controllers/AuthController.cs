using MarketBackEnd.Auth;
using MarketBackEnd.DTOs.User;
using MarketBackEnd.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketBackEnd.Controllers
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
    }
}
