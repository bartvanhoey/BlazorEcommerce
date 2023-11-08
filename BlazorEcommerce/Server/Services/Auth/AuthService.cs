using Server.Data;
using Shared;

namespace Server.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseContext _db;

        public AuthService(DatabaseContext databaseContext) => _db = databaseContext;

        public async Task<ServiceResponse<int>> Register(User user, string password)
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

        public async Task<bool> UserExists(string email)
        => await _db.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>{
                Data = "token"
            };

            await Task.CompletedTask;
            return response;
        }
    }

}