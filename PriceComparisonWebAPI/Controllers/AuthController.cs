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
    [Authorize(Policy = "AdminRights")]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;


        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseModel))]
        public async Task<JsonResult> Login([FromBody] LoginRequestModel request)
        {
            return await _authService.LoginAsync(request);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> Register([FromBody] RegisterRequestModel request)
        {
            return await _authService.RegisterAsync(request);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefreshTokenResponseModel))]
        public async Task<JsonResult> RefreshToken([FromBody] RefreshTokenResponseModel request)
        {
            return await _authService.RefreshTokenAsync(request);
        }

        [Authorize(Policy = "AdminRights")]
        [HttpPost("update-roles")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateUserRoles([FromBody] UpdateUserRolesRequestModel request)
        {
            var result = await _authService.UpdateUserRolesAsync(request);
            return result.IsSuccess ?
                GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK) :
                GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
        }

        [Authorize(Policy = "AdminRights")]
        [HttpGet("get-all-roles")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
        public async Task<JsonResult> GetAllRoles()
        {
            var result = await _authService.GetAllRolesAsync();
            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [Authorize(Policy = "AdminRights")]
        [HttpPost("crate-role")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> GetAllRoles(string roleName)
        {
            var result = await _authService.CreateRoleAsync(roleName);
            return result.IsSuccess ?
                 GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK) :
                 GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
        }
    }
}
