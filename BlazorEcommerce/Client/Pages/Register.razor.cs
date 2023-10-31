using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorEcommerce.Client.Pages
{
    public class RegisterBase : ComponentBase
    {
        protected UserRegisterModel UserRegisterModel = new();

        protected void HandleValidSubmitAsync(){
            Console.WriteLine($"Register User: {UserRegisterModel.Email}");
        }
    }
}