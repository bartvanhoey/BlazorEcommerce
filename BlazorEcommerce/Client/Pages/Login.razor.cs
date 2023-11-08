using Blazored.LocalStorage;

namespace BlazorEcommerce.Client.Pages
{
    public class LoginBase : ComponentBase
    {
        public UserLoginModel UserLoginModel { get; set; } = new();

        [Inject] protected ILocalStorageService? LocalStorage { get; set; }

        [Inject] protected IAuthService? AuthService { get; set; }

        [Inject] public NavigationManager? NavigationManager { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        protected async void HandleValidSubmitAsync()
        {
            var result = await AuthService!.LoginAsync(UserLoginModel);
            if (result.Success)
            {
                ErrorMessage = string.Empty;
                await LocalStorage!.SetItemAsync("authToken", result.Data);
                NavigationManager!.NavigateTo("/");
            }
            else
            {
                ErrorMessage = result.Message;
            }
        }
    }
}