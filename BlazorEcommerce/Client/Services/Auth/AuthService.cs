namespace Client.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;

        public AuthService(HttpClient http) => _http = http;

        public async Task<ServiceResponse<bool>> ChangePasswordAsync(UserChangePasswordModel model)
        {
            var response = await _http.PostAsJsonAsync("api/auth/change-password", model);
            return await response.Content
                       .ReadFromJsonAsync<ServiceResponse<bool>>()
                   ?? new ServiceResponse<bool>() { Success = false, Message = "Change Password Failed" };
        }

        public async Task<ServiceResponse<string>> LoginAsync(UserLoginModel model)
            => await (await _http.PostAsJsonAsync("api/auth/login", model)).Content.ReadFromJsonAsync<ServiceResponse<string>>()
                ?? new ServiceResponse<string>() { Success = false, Message = "Login Failed" };

        public async Task<ServiceResponse<int>> RegisterAsync(UserRegisterModel model)
        => await (await _http.PostAsJsonAsync("api/auth/register", model)).Content.ReadFromJsonAsync<ServiceResponse<int>>()
                ?? new ServiceResponse<int>() { Success = false, Message = "Registration Failed" };
    }
}