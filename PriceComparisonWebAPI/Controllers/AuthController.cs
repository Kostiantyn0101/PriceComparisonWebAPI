using BLL.Services.Auth;
using Domain.Models.Exceptions;
using Domain.Models.Request.Auth;
using Domain.Models.Response;
using Domain.Models.Response.Auth;
using Domain.Models.SuccessCodes;
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

        [AllowAnonymous]
        [HttpPost("update-roles")]
        public async Task<JsonResult> UpdateUserRoles([FromBody] UpdateUserRolesRequestModel request)
        {
            var result = await _authService.UpdateUserRolesAsync(request);
            return result.IsSuccess ?
                GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK) :
                GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
        }

        [AllowAnonymous]
        [HttpGet("get-all-roles")]
        public async Task<JsonResult> GetAllRoles()
        {
            var result = await _authService.GetAllRolesAsync();
            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [AllowAnonymous]
        [HttpPost("crate-role")]
        public async Task<JsonResult> GetAllRoles(string roleName)
        {
            var result = await _authService.CreateRoleAsync(roleName);
            return result.IsSuccess ?
                 GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK) :
                 GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
        }
    }
}
