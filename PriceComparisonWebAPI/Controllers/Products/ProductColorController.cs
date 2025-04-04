using BLL.Services.ProductServices;
using Domain.Models.Exceptions;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers.Products
{
    [Authorize(Policy = "AdminRights")]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
    public class ProductColorController : ControllerBase
    {
        private readonly ILogger<ProductColorController> _logger;
        private readonly IColorService _colorService;


        public ProductColorController(ILogger<ProductColorController> logger, IColorService colorService)
        {
            _logger = logger;
            _colorService = colorService;
        }


        [AllowAnonymous]
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ColorResponseModel>))]
        public async Task<JsonResult> GetAllColors()
        {
            var result = await _colorService.GetFromConditionAsync(c => true);
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

        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ColorResponseModel))]
        public async Task<JsonResult> GetColorById(int id)
        {
            var result = await _colorService.GetFromConditionAsync(c => c.Id == id);
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
        [HttpGet("byproductgroup")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductColorResponseModel>))]
        public async Task<JsonResult> GetColorsByProductGroupId([FromQuery] ProductColorRequestModel requestModel)
        {
            var result = await _colorService.GetByProductGroupIdAsync(requestModel);
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
        public async Task<JsonResult> CreateColor([FromBody] ColorCreateRequestModel request)
        {
            var result = await _colorService.CreateAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK);
        }

        [Authorize(Policy = "AdminRights")]
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateColor([FromBody] ColorUpdateRequestModel request)
        {
            var result = await _colorService.UpdateAsync(request);
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
        public async Task<JsonResult> DeleteColor(int id)
        {
            var result = await _colorService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }
    }
}
