using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Server.Data;
using Shared;

namespace Server.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseContext _db;
        private readonly IConfiguration _configuration;

        public AuthService(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _db = databaseContext;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<int>> RegisterAsync(User user, string password)
        {
            if (await UserExists(user.Email))
            {
                return new ServiceResponse<int> { Success = false, Message = "Email already exists" };
            }

            var (passwordHash, passwordSalt) = CreatePasswordHash(password);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new ServiceResponse<int> { Data = user.Id, Message = "Registration Successful!" };
        }


        public async Task<bool> UserExists(string email)
        => await _db.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());

        public async Task<ServiceResponse<string>> LoginAsync(string email, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
            }
            else if (IsPasswordCorrect(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Data = CreateToken(user);
            }
            else
            {
                response.Success = false;
                response.Message = "wrong password";
            }


            return response;
        }

        private string? CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var appSettingsToken = _configuration.GetSection("AppSettings:Token")?.Value;
            if (appSettingsToken == null) return string.Empty;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettingsToken));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var jwToken = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwToken);
        }

        private bool IsPasswordCorrect(string? password, byte[]? passwordHash, byte[]? passwordSalt)
        {
            if (password == null) return false;
            if (passwordHash == null) return false;
            if (passwordSalt == null) return false;

            using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        private (byte[]? passwordHash, byte[]? passwordSalt) CreatePasswordHash(string password)
        {
            byte[]? passwordHash = null;
            byte[]? passwordSalt = null;

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            return (passwordHash, passwordSalt);
        }

        public async Task<ServiceResponse<bool>> ChangePasswordAsync(int userId, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return new ServiceResponse<bool> { Success = false, Message = "User not found" };

            var (passwordHash, passwordSalt) = CreatePasswordHash(password);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _db.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true, Message = "Password changed!" };
        }
    }

}