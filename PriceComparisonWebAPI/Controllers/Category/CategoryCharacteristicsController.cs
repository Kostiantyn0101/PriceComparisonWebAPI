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
    public class CategoryCharacteristicsController : ControllerBase
    {
        private readonly ILogger<CategoryCharacteristicsController> _logger;
        private readonly ICategoryCharacteristicService _categoryCharacteristicService;
        private readonly IMapper _mapper;

        public CategoryCharacteristicsController(
            ICategoryCharacteristicService categoryCharacteristicService,
            ILogger<CategoryCharacteristicsController> logger,
            IMapper mapper
            )
        {
            _categoryCharacteristicService = categoryCharacteristicService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{categoryId}")]
        public async Task<JsonResult> GetCategoryCharacteristics(int categoryId)
        {
            var characteristics = await _categoryCharacteristicService
               .GetQuery()
               .Where(x => x.CategoryId == categoryId)
               .Include(x => x.Characteristic)
               .ToListAsync();

            if (!characteristics.Any())
            {
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.NotFound,
                    StatusCodes.Status400BadRequest);
            }

            return new JsonResult(_mapper.Map<List<CategoryCharacteristicResponseModel>>(characteristics))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> AddCategoryCharacteristic([FromBody] CategoryCharacteristicRequestModel request)
        {

            var validationError = ValidateRequest(request, "add");

            if (validationError != null)
                return validationError;

            var result = await _categoryCharacteristicService.CreateMultipleAsync(request);

            return ProcessResult(result, "add");
        }

        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteCategoryCharacteristics([FromBody] CategoryCharacteristicRequestModel request)
        {
            var validationError = ValidateRequest(request, "delete");
            if (validationError != null)
                return validationError;

            var result = await _categoryCharacteristicService.DeleteMultipleAsync(request);
            return ProcessResult(result, "delete");

        }

        private JsonResult ProcessResult(IEnumerable<OperationDetailsResponseModel> result, string operationType)
        {
            if (result.All(r => r.IsError))
            {
                _logger.LogError("All errors occurred during {Operation}: {Errors}",
                                 operationType,
                                 string.Join(", ", result.Select(r => r.Message)));
                var errorCode = operationType == "add" ? AppErrors.General.CreateError : AppErrors.General.DeleteError;
                var errorMessage = operationType == "add" ? "No characteristic was successfully added." : "No characteristic was successfully deleted.";
                return GeneralApiResponseModel.GetJsonResult(errorCode, StatusCodes.Status400BadRequest, errorMessage);
            }

            var successCount = result.Count(r => !r.IsError);
            var successCode = operationType == "add" ? AppSuccessCodes.CreateSuccess : AppSuccessCodes.DeleteSuccess;
            var successMessage = operationType == "add"
                ? $"{successCount} characteristic{(successCount > 1 ? "s" : "")} successfully added."
                : $"{successCount} characteristic{(successCount > 1 ? "s" : "")} successfully deleted.";
            return GeneralApiResponseModel.GetJsonResult(successCode, StatusCodes.Status200OK, successMessage);
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
