using MarketBackEnd.EmailSender.Services.Interfaces;
using MarketBackEnd.Shared.Data;
using MarketBackEnd.Shared.Model;
using MarketBackEnd.Users.Auth.DTOs;
using MarketBackEnd.Users.Auth.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MarketBackEnd.Users.Auth.Services.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthRepository(ApplicationDbContext db, IConfiguration configuration, IEmailService emailService)
        {
            _db = db;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
            }
            else if (user.IsActive == false)
            {
                response.Success = false;
                response.Message = "User is not active";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password";
            }
            else
            {
                response.Data = GenerateToken(user);
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();

            if (await UserExists(user.Email))
            {
                response.Success = false;
                response.Message = "User already exists";
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            response.Data = user.Id;
            return response;
        }

        public async Task<ServiceResponse<string>> ForgotPassword(string email)
        {
            var response = new ServiceResponse<string>();
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            try
            {
                var token = GenerateResetToken();

                user.PasswordResetToken = token;
                user.ResetTokenExpires = DateTime.Now.AddHours(1);
                await _db.SaveChangesAsync();

                await _emailService.SendPasswordResetEmail(user.Email, token);

                response.Success = true;
                response.Message = "Password reset token sent.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Failed to send password reset email: " + ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<string>> ResetPassword(string email, string token, string newPassword)
        {
            var response = new ServiceResponse<string>();
            var user = await _db.Users.FirstOrDefaultAsync(x => x.PasswordResetToken == token && x.ResetTokenExpires > DateTime.Now && x.Email == email);

            if (user == null)
            {
                response.Success = false;
                response.Message = "Invalid or expired token";
                return response;
            }

            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _db.SaveChangesAsync();

            response.Success = true;
            response.Message = "Password has been reset";
            return response;
        }

        public string GenerateResetToken()
        {
            const string chars = "0123456789";
            var random = new Random();
            var token = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return token;
        }


        public async Task<bool> UserExists(string email)
        {
            if (await _db.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower()))
            {
                return true;
            }
            return false;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
