namespace BlazorEcommerce.Client.Pages
{
    public class RegisterBase : ComponentBase
    {
        protected UserRegisterModel UserRegisterModel = new();

        [Inject] protected IAuthService? AuthService { get; set; }

        public string Message { get; set; } = string.Empty;
        public string MessageCssClass { get; set; } = string.Empty;

        protected async void HandleValidSubmitAsync()
        {
            var result = await AuthService!.RegisterAsync(UserRegisterModel);
            Message = result.Message;
            MessageCssClass = result.Success ? "text-success" : "text-danger";
            StateHasChanged();
        }
    }
}