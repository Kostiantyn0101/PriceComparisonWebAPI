using BLL.Services.FilterServices;
using Domain.Models.Exceptions;
using Domain.Models.Request.Filters;
using Domain.Models.Response;
using Domain.Models.Response.Filters;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers.Filters
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
    public class FilterController : ControllerBase
    {
        private readonly ILogger<FilterController> _logger;
        private readonly IFilterService _filterService;


        public FilterController(ILogger<FilterController> logger, IFilterService filterService)
        {
            _logger = logger;
            _filterService = filterService;
        }

        
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FilterResponseModel))]
        public async Task<JsonResult> GetFilterById(int id)
        {
            var result = await _filterService.GetFromConditionAsync(x => x.Id == id);
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

        [Authorize(Policy = "AdminRights")]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateFilter([FromBody] FilterCreateRequestModel request)
        {
            var createResult = await _filterService.CreateAsync(request);
            if (!createResult.IsSuccess)
            {
                _logger.LogError(createResult.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, createResult.ErrorMessage);
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK);
        }

        [Authorize(Policy = "AdminRights")]
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateFilter([FromBody] FilterUpdateRequestModel request)
        {
            var updateResult = await _filterService.UpdateAsync(request);
            if (!updateResult.IsSuccess)
            {
                _logger.LogError(updateResult.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest, updateResult.ErrorMessage);
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK);
        }

        [Authorize(Policy = "AdminRights")]
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteFilter(int id)
        {
            var deleteResult = await _filterService.DeleteAsync(id);
            if (!deleteResult.IsSuccess)
            {
                _logger.LogError(deleteResult.Exception, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError, StatusCodes.Status400BadRequest, deleteResult.ErrorMessage);
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }
    }
}
