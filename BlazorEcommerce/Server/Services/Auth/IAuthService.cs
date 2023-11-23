using Shared;

namespace Server.Services.Auth
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> RegisterAsync(User user, string password);
        Task<ServiceResponse<string>> LoginAsync(string email, string password);
        Task<ServiceResponse<bool>> ChangePasswordAsync(int userId, string password);
        Task<bool> UserExists(string email);
        string? GetUserEmail();
        Task<User?> GetUserByEmailAsync(string email);
        int GetUserId();

    }

}