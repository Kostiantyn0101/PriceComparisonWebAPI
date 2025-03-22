using AutoMapper;
using BLL.Services.CategoryCharacteristicService;
using Domain.Models.Exceptions;
using Domain.Models.Request.Categories;
using Domain.Models.Response;
using Domain.Models.Response.Categories;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PriceComparisonWebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
    public class CategoryCharacteristicsController : ControllerBase
    {
        private readonly ILogger<CategoryCharacteristicsController> _logger;
        private readonly ICategoryCharacteristicService _categoryCharacteristicService;


        public CategoryCharacteristicsController(
            ICategoryCharacteristicService categoryCharacteristicService,
            ILogger<CategoryCharacteristicsController> logger
            )
        {
            _categoryCharacteristicService = categoryCharacteristicService;
            _logger = logger;
        }


        [HttpGet("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryCharacteristicResponseModel>))]
        public async Task<JsonResult> GetCategoryCharacteristics(int categoryId)
        {
            var serviceResult = await _categoryCharacteristicService.GetMappedCharacteristicsAsync(categoryId);
            if (!serviceResult.IsSuccess)
            {
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest, serviceResult.ErrorMessage);
            }

            return new JsonResult(serviceResult.Data)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> AddCategoryCharacteristic([FromBody] CategoryCharacteristicRequestModel request)
        {
            var validationError = ValidateRequest(request, "add");
            if (validationError != null)
                return validationError;

            var serviceResult = await _categoryCharacteristicService.CreateMultipleAsync(request);
            if (!serviceResult.IsSuccess)
            {
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, serviceResult.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK);
        }


        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateCategoryCharacteristic([FromBody] CategoryCharacteristicUpdateRequestModel request)
        {
            var result = await _categoryCharacteristicService.UpdateAsync(request);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError,
                       StatusCodes.Status400BadRequest, result.ErrorMessage);
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK);
        }


        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteCategoryCharacteristics([FromBody] CategoryCharacteristicRequestModel request)
        {
            var validationError = ValidateRequest(request, "delete");
            if (validationError != null)
                return validationError;

            var serviceResult = await _categoryCharacteristicService.DeleteMultipleAsync(request);
            if (!serviceResult.IsSuccess)
            {
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError, StatusCodes.Status400BadRequest, serviceResult.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }


        private JsonResult ValidateRequest(CategoryCharacteristicRequestModel request, string operationType)
        {
            if (request == null || request.CharacteristicIds == null || !request.CharacteristicIds.Any())
            {
                var errorCode = operationType == "add" ? AppErrors.General.CreateError : AppErrors.General.DeleteError;
                return GeneralApiResponseModel.GetJsonResult(
                    errorCode,
                    StatusCodes.Status400BadRequest,
                    "Request must contain category ID and at least one characteristic ID.");
            }
            return null;
        }
    }
}
