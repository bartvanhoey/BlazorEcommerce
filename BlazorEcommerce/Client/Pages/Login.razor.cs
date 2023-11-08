namespace BlazorEcommerce.Client.Pages
{
    public class LoginBase:  ComponentBase
    {
        public UserLoginModel UserLoginModel { get; set; } = new();


        protected async void HandleValidSubmitAsync()
        {
            
        }
    
    }
}