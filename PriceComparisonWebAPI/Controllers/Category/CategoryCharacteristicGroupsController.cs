using BLL.Services.CategoryServices;
using Domain.Models.Exceptions;
using Domain.Models.Request.Categories;
using Domain.Models.Response;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers.Category
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryCharacteristicGroupsController : ControllerBase
    {
        private readonly ILogger<CategoryCharacteristicGroupsController> _logger;
        private readonly ICategoryCharacteristicGroupService _categoryCharacteristicGroupService;

        public CategoryCharacteristicGroupsController(ICategoryCharacteristicGroupService categoryCharacteristicGroupService,
            ILogger<CategoryCharacteristicGroupsController> logger)
        {
            _categoryCharacteristicGroupService = categoryCharacteristicGroupService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> GetCategoryCharacteristicGroupById(int id)
        {
            var categoryCharacteristicGroup = (await _categoryCharacteristicGroupService
                .GetFromConditionAsync(x => x.Id == id)).FirstOrDefault();

            if (categoryCharacteristicGroup == null)
            {
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.NotFound,
                    StatusCodes.Status400BadRequest);
            }
            return new JsonResult(categoryCharacteristicGroup)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateCategoryCharacteristicGroup([FromBody] CategoryCharacteristicGroupCreateRequestModel model)
        {
            var result = await _categoryCharacteristicGroupService.CreateAsync(model);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError,
                    StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(
                AppSuccessCodes.CreateSuccess,
                StatusCodes.Status200OK);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateCategoryCharacteristicGroup([FromBody] CategoryCharacteristicGroupRequestModel model)
        {
            var result = await _categoryCharacteristicGroupService.UpdateAsync(model);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError,
                    StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK);
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteCategoryCharacteristicGroup(int id)
        {
            var result = await _categoryCharacteristicGroupService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError,
                    StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }
    }
}
