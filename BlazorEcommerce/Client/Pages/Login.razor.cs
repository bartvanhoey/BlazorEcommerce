using Blazored.LocalStorage;
using Microsoft.AspNetCore.WebUtilities;

namespace BlazorEcommerce.Client.Pages
{
    public class LoginBase : ComponentBase
    {
        public UserLoginModel UserLoginModel { get; set; } = new();

        [Inject] protected ILocalStorageService? LocalStorage { get; set; }

        [Inject] protected IAuthService? AuthService { get; set; }

        [Inject] public NavigationManager? NavigationManager { get; set; }

        [Inject] protected AuthenticationStateProvider? AuthenticationStateProvider  { get; set; }

        public string ReturnUrl { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        protected async void HandleValidSubmitAsync()
        {
            var result = await AuthService!.LoginAsync(UserLoginModel);
            if (result.Success)
            {
                ErrorMessage = string.Empty;
                await LocalStorage!.SetItemAsync("authToken", result.Data);
                await AuthenticationStateProvider?.GetAuthenticationStateAsync()!;
                NavigationManager!.NavigateTo(ReturnUrl);
            }
            else
            {
                ErrorMessage = result.Message;
            }
        }

        protected override void OnInitialized()
        {
            var uri = NavigationManager?.ToAbsoluteUri(NavigationManager.Uri);
            if (uri != null && QueryHelpers.ParseQuery(uri.Query).TryGetValue("returnUrl", out var returnUrl))
            {
                ReturnUrl = returnUrl;
            }

        }
    }
}