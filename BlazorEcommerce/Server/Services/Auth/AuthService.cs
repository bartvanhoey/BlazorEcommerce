using Server.Data;
using Shared;

namespace Server.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseContext _db;

        public AuthService(DatabaseContext databaseContext) => _db = databaseContext;

        public Task<ServiceResponse<int>> Register(User user, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UserExists(string email) 
        => await _db.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

}