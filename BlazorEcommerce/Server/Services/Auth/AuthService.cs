using Server.Data;
using Shared;

namespace Server.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly DatabaseContext databaseContext;

        public AuthService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public Task<ServiceResponse<int>> Register(User user, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserExists(string email)
        {
            throw new NotImplementedException();
        }
    }

}