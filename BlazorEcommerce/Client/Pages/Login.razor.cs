using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Pages
{
    public class LoginBase :  ComponentBase
    {
        public UserLoginModel UserLoginModel { get; set; } = new();


        protected async void HandleValidSubmitAsync()
        {
            
        }
    
    }
}