
namespace Client.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ServiceResponse<int>> RegisterAsync(UserRegisterModel model)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", model);
            return await response.Content.ReadFromJsonAsync<ServiceResponse<int>>() ?? new ServiceResponse<int>() { Success = false, Message = "Something went wrong" };
        }
    }


}