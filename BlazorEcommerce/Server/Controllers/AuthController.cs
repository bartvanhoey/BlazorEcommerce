using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Services.Auth;
using Shared;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterModel model)
        {
            var response = await _authService.RegisterAsync(new User { Email = model.Email }, model.Password);

            if (!response.Success) return BadRequest(response);
            
            return Ok(response);
        }

      [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginModel model)
        {
            var response = await _authService.LoginAsync(model.Email, model.Password);
            if (!response.Success) return BadRequest(response);
            
            return Ok(response);
        }

    }
}