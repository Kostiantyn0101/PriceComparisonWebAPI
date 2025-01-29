using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Domain.Models.Auth;
using Domain.Models.Configuration;
using Domain.Models.DBModels;
using Domain.Models.Exceptions;
using Domain.Models.Identity;
using Domain.Models.Response;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BLL.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly UserManager<ApplicationUserDBModel> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        //private readonly IEmailSender _emailService;
        //private readonly IHostEnvironment _hostEnvironment;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IConfiguration _configuration;
        //private readonly IUserSettingsService _userSettingsService;

        public AuthService(IOptions<JwtConfiguration> jwtConfiguration, UserManager<ApplicationUserDBModel> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _jwtConfiguration = jwtConfiguration.Value;
            _userManager = userManager;
            _roleManager = roleManager;
        }



        public async Task<JsonResult> LoginAsync(LoginRequestModel request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.Username!) ?? await _userManager.FindByEmailAsync(request.Username!);

                if (user is null)
                {
                    return new JsonResult(new GeneralApiResponseModel()
                    {
                        ReturnCode = AppErrors.Auth.UserNotFound,
                        Message = "User name not found"
                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                if (!await _userManager.CheckPasswordAsync(user, request.Password!))
                {
                    return new JsonResult(new GeneralApiResponseModel
                    {
                        ReturnCode = AppErrors.Auth.PasswordIncorrect,
                        Message = "Password Incorrect"
                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                if (user.Provider != Consts.LoginProviders.Password)
                {
                    return new JsonResult(new GeneralApiResponseModel
                    {
                        ReturnCode = AppErrors.Auth.ProviderMismatch,
                        Message = "Provider Mismatch"
                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };

                }

                var token = await CreateToken(user);
                var refreshToken = GenerateRefreshToken();
                user.RefreshToken = refreshToken;

                user.RefreshTokenExpiryTime = request.RememberMe ? DateTimeOffset.Now.AddDays(_jwtConfiguration.RememberMeRefreshTokenLifetimeHours)
                                                                         : DateTimeOffset.Now.AddHours(_jwtConfiguration.DefaultRefreshTokenLifetimeHours);

                await _userManager.UpdateAsync(user);

                return new JsonResult(new LoginResponseModel
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken
                })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                return new JsonResult(new GeneralApiResponseModel
                {
                    ReturnCode = AppErrors.General.InternalServerError,
                    Message = ex.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        public async Task<JsonResult> RegisterAsync(RegisterRequestModel request)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(request.Username) || request.Username.Length < Consts.UsernameMinLength)
            {
                errors.Add(Consts.UsernameLengthValidationError);
            }

            if (string.IsNullOrWhiteSpace(request.Email) || !Regex.IsMatch(request.Email, Consts.EmailRegex))
            {
                errors.Add(Consts.EmailValidationError);
            }

            if (string.IsNullOrWhiteSpace(request.Password) || !Regex.IsMatch(request.Password, Consts.PasswordRegex))
            {
                errors.Add(Consts.PasswordValidationError);
            }

            if (errors.Any())
            {
                return new JsonResult(new GeneralApiResponseModel
                {
                    ReturnCode = string.Join("|", errors),
                    Message = "Register error"
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            try
            {
                var userByEmail = await _userManager.FindByEmailAsync(request.Email!);
                if (userByEmail is not null)
                {
                    return new JsonResult(new GeneralApiResponseModel
                    {
                        ReturnCode = AppErrors.Auth.EmailExists,
                        Message = "Email already exists"
                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                var userByUserName = await _userManager.FindByNameAsync(request.Username!);

                if (userByUserName is not null)
                {
                    return new JsonResult(new GeneralApiResponseModel
                    {
                        ReturnCode = AppErrors.Auth.UserNameExists,
                        Message = "User Name already exits"
                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

                var user = new ApplicationUserDBModel
                {
                    Email = request.Email,
                    UserName = request.Username,
                    Provider = Consts.LoginProviders.Password,
                };

                var result = await _userManager.CreateAsync(user, request.Password!);
                if (!await _roleManager.RoleExistsAsync(Role.User))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int> { Name = Role.User });
                }

                await _userManager.AddToRoleAsync(user, Role.User);

                if (!result.Succeeded)
                {
                    return new JsonResult(new GeneralApiResponseModel
                    {
                        ReturnCode = AppErrors.Auth.AddToRoleFailed,
                        Message = "Adding to roles failed "
                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

                //var userSettings = new UserSettingsDBModel
                //{
                //    UserId = user.Id
                //};
                //var resultCreating = await _userSettingsService.CreateAsync(userSettings);

                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var cryptToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                //string filePath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "TemplateHtml", "ConfirmEmail.html");
                //var sendForm = "";
                //if (System.IO.File.Exists(filePath))
                //{
                //    sendForm = System.IO.File.ReadAllText(filePath);
                //}
                //else
                //{
                //    return new JsonResult(new GeneralApiResponseModel
                //    {
                //        ReturnCode = AppErrors.Auth.PathNotFound,
                //        Message = "Path Not Found"
                //    })
                //    {
                //        StatusCode = StatusCodes.Status400BadRequest
                //    };
                //}

                //var confirmationLink = $"{_configuration["SelfServerUrl"]}EmailConfirmation?email={user.Email}&token={cryptToken}";
                //await _emailService.SendEmailAsync(user.Email, "Confirm your email", sendForm.Replace("Your reference", confirmationLink));


                return new JsonResult(new GeneralApiResponseModel
                {
                    ReturnCode = AppSuccessCodes.CreateSuccess,
                    Message = "Registrated successfully"
                })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch
            {
                return new JsonResult(new GeneralApiResponseModel
                {
                    ReturnCode = AppErrors.General.InternalServerError,
                    Message = "Internal Server Error"
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        public async Task<JsonResult> RefreshTokenAsync(RefreshTokenResponseModel request)
        {
            try
            {
                var accessToken = request.AccessToken;
                var refreshToken = request.RefreshToken;

                var principal = GetPrincipalFromExpiredToken(accessToken);
                if (principal == null)
                {
                    return new JsonResult(new GeneralApiResponseModel
                    {
                        ReturnCode = AppErrors.Auth.RefreshInvalidToken,
                        Message = "Invalid Token"
                    })
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                }

                var username = principal.Identity!.Name!;

                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return new JsonResult(new GeneralApiResponseModel
                    {
                        ReturnCode = AppErrors.Auth.UserNotFound,
                        Message = "User Not Found"
                    })
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                if (user.RefreshToken != request.RefreshToken)
                {
                    return new JsonResult(new GeneralApiResponseModel
                    {
                        ReturnCode = AppErrors.Auth.RefreshTokenMismatch,
                        Message = "Refresh Token Mismatch"
                    })
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                }
                if (user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    return new JsonResult(new GeneralApiResponseModel
                    {
                        ReturnCode = AppErrors.Auth.RefreshTokenExpired,
                        Message = "Refresh Token Expired"
                    })
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                }
                var newAccessToken = await CreateToken(user);
                var newRefreshToken = GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                await _userManager.UpdateAsync(user);

                return new JsonResult(new RefreshTokenResponseModel
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                    RefreshToken = newRefreshToken
                })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                return new JsonResult(new GeneralApiResponseModel
                {
                    ReturnCode = AppErrors.General.InternalServerError,
                    Message = ex.Message
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }


        private async Task<JwtSecurityToken> CreateToken(ApplicationUserDBModel user)
        {
            try
            {
                var authClaims = await GetClaims(user);
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key!));

                var token = new JwtSecurityToken(
                    issuer: _jwtConfiguration.Issuer,
                    audience: _jwtConfiguration.Audience,
                    expires: DateTime.Now.AddHours(_jwtConfiguration.AccessTokenLifetimeMin),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

                return token;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<List<Claim>> GetClaims(ApplicationUserDBModel user)
        {
            var authClaims = new List<Claim>
        {
            new(ClaimTypes.Sid, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Any())
            {
                authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
            }

            return authClaims;
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key!)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException(AppErrors.Auth.RefreshInvalidToken);

            return principal;
        }
    }
}
