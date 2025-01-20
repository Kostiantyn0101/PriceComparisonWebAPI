using BLL.Services.Auth;
using Domain.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<JsonResult> Login([FromBody] LoginRequestModel request)
        {
            return await _authService.LoginAsync(request);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<JsonResult> Register([FromBody] RegisterRequestModel request)
        {
            return await _authService.RegisterAsync(request);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<JsonResult> RefreshToken([FromBody] RefreshTokenResponseModel request)
        {
            return await _authService.RefreshTokenAsync(request);
        }
    }
}
