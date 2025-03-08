using BLL.Services.FilterServices;
using Domain.Models.Exceptions;
using Domain.Models.Request.Filters;
using Domain.Models.Response;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers.Filters
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    public class FilterCriterionController : ControllerBase
    {
        private readonly ILogger<FilterCriterionController> _logger;
        private readonly IFilterCriterionService _criterionService;

        public FilterCriterionController(ILogger<FilterCriterionController> logger, IFilterCriterionService criterionService)
        {
            _logger = logger;
            _criterionService = criterionService;
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> GetFilterCriterionById(int id)
        {
            var result = await _criterionService.GetFromConditionAsync(x => x.Id == id);
            if (result == null || !result.Any())
            {
                _logger.LogError(AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.NotFound,
                    StatusCodes.Status400BadRequest
                );
            }

            return new JsonResult(result.First())
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateFilterCriterion([FromBody] FilterCriterionCreateRequestModel request)
        {
            var createResult = await _criterionService.CreateAsync(request);
            if (!createResult.IsSuccess)
            {
                _logger.LogError(createResult.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.CreateError,
                    StatusCodes.Status400BadRequest,
                    createResult.ErrorMessage
                );
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateFilterCriterion([FromBody] FilterCriterionUpdateRequestModel request)
        {
            var updateResult = await _criterionService.UpdateAsync(request);
            if (!updateResult.IsSuccess)
            {
                _logger.LogError(updateResult.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.UpdateError,
                    StatusCodes.Status400BadRequest,
                    updateResult.ErrorMessage
                );
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK);
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteFilterCriterion(int id)
        {
            var deleteResult = await _criterionService.DeleteAsync(id);
            if (!deleteResult.IsSuccess)
            {
                _logger.LogError(deleteResult.Exception, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.DeleteError,
                    StatusCodes.Status400BadRequest,
                    deleteResult.ErrorMessage
                );
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }
    }
}
