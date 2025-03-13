using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Request.Auth;
using Domain.Models.Response;
using Domain.Models.Response.Auth;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BLL.Services.Auth
{
    public interface IAuthService
    {
        Task<JsonResult> RegisterAsync(RegisterRequestModel request);
        Task<JsonResult> LoginAsync(LoginRequestModel request);
        Task<JsonResult> RefreshTokenAsync(RefreshTokenResponseModel request);
        Task<OperationResultModel<bool>> UpdateUserRolesAsync(UpdateUserRolesRequestModel request);
        Task<List<string>> GetAllRolesAsync();
        Task<OperationResultModel<bool>> CreateRoleAsync(string roleName);
    }
}
