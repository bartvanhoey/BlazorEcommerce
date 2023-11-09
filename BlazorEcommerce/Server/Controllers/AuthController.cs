using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        
        [HttpPost("change-password"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword(string password)
        {
            var response = await _authService.ChangePasswordAsync(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), password);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

    }
}