﻿using Domain.Models.Response;
using Domain.Models.SuccessCodes;
using Domain.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using BLL.Services.SellerServices;
using Domain.Models.Request.Seller;
using Domain.Models.Response.Seller;
using Microsoft.AspNetCore.Authorization;

namespace PriceComparisonWebAPI.Controllers.Categories
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
    public class SellerController : ControllerBase
    {
        private readonly ILogger<SellerController> _logger;
        private readonly ISellerService _sellerService;


        public SellerController(ILogger<SellerController> logger, ISellerService sellerService)
        {
            _logger = logger;
            _sellerService = sellerService;
        }


        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SellerResponseModel))]
        public async Task<JsonResult> GetSellerById(int id)
        {
            var result = await _sellerService.GetFromConditionAsync(s => s.Id == id);
            if (result == null || !result.Any())
            {
                _logger.LogError(AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }
            return new JsonResult(result.First())
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [AllowAnonymous]
        [HttpGet("getByUserId/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SellerResponseModel))]
        public async Task<JsonResult> GetSellerByUserId(int userId)
        {
            var result = await _sellerService.GetFromConditionAsync(s => s.UserId == userId);
            if (result == null || !result.Any())
            {
                _logger.LogError(AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }
            return new JsonResult(result.First())
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [AllowAnonymous]
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SellerResponseModel>))]
        public async Task<JsonResult> GetSellers()
        {
            var result = await _sellerService.GetFromConditionAsync(s => true);
            if (result == null || !result.Any())
            {
                _logger.LogError(AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }
            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [Authorize(Policy = "AdminRights")]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateSeller([FromForm] SellerCreateRequestModel request)
        {
            var result = await _sellerService.CreateAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK);
        }

        [Authorize(Policy = "SellerAndAdminRights")]
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateSeller([FromForm] SellerUpdateRequestModel request)
        {
            var result = await _sellerService.UpdateAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK);
        }

        [Authorize(Policy = "AdminRights")]
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteSeller(int id)
        {
            var result = await _sellerService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }
    }
}
