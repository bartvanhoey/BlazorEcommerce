using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Services.Auth
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> RegisterAsync(UserRegisterModel model);
        Task<ServiceResponse<string>> LoginAsync(UserLoginModel model);
    }
}