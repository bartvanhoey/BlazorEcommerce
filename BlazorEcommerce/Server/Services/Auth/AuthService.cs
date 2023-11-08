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


        public async Task<bool> UserExists(string email)
        => await _db.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            response.Data = "token";

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
            }
            else if (IsPasswordNotCorrect(password, user.PasswordHash, user.PasswordSalt))
            {

            }

            await Task.CompletedTask;
            return response;
        }

        private bool IsPasswordNotCorrect(string? password, byte[]? passwordHash, byte[]? passwordSalt)
        {
            if (password == null) return false;
            if (passwordHash == null) return false;
            if (passwordSalt == null) return false;

            using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
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

    }

}