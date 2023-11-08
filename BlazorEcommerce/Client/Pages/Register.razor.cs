using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorEcommerce.Client.Pages
{
    public class RegisterBase : ComponentBase
    {
        protected UserRegisterModel UserRegisterModel = new();

        [Inject] protected IAuthService? AuthService { get; set; }

        public string ErrorMessage { get; set; }  = string.Empty;

        protected async void HandleValidSubmitAsync(){
           var result =  await AuthService!.RegisterAsync(UserRegisterModel);
           ErrorMessage = result.Success ? string.Empty : result.Message;
        }
    }
}