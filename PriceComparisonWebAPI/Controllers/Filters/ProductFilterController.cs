using Domain.Models.Request.Filters;
using Domain.Models.Response;
using Domain.Models.SuccessCodes;
using Domain.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using BLL.Services.FilterServices;

namespace PriceComparisonWebAPI.Controllers.Filters
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    public class ProductFilterController : ControllerBase
    {
        private readonly ILogger<ProductFilterController> _logger;
        private readonly IProductFilterService _productFilterService;

        public ProductFilterController(ILogger<ProductFilterController> logger, IProductFilterService productFilterService)
        {
            _logger = logger;
            _productFilterService = productFilterService;
        }

        [HttpGet("{productId}/{filterId}")]
        public async Task<JsonResult> GetProductFilterByIds(int productId, int filterId)
        {
            var result = await _productFilterService.GetFromConditionAsync(x => x.ProductId == productId && x.FilterId == filterId);
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

        [HttpGet("filtersByProduct/{productId}")]
        public async Task<JsonResult> GetFiltersByProduct(int productId)
        {
            var result = await _productFilterService.GetFiltersByProductIdAsync(productId);
            if (result == null || !result.Any())
            {
                _logger.LogError(AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.NotFound,
                    StatusCodes.Status400BadRequest
                );
            }

            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [HttpGet("productsByFilter/{filterId}")]
        public async Task<JsonResult> GetProductsByFilter(int filterId)
        {
            var result = await _productFilterService.GetProductsByFilterIdAsync(filterId);
            if (result == null || !result.Any())
            {
                _logger.LogError(AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.NotFound,
                    StatusCodes.Status400BadRequest
                );
            }

            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateProductFilter([FromBody] ProductFilterCreateRequestModel request)
        {
            var createResult = await _productFilterService.CreateAsync(request);
            if (!createResult.IsSuccess)
            {
                _logger.LogError(createResult.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.CreateError,
                    StatusCodes.Status400BadRequest,
                    createResult.ErrorMessage
                );
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess,StatusCodes.Status200OK);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateProductFilter([FromBody] ProductFilterUpdateRequestModel request)
        {
            var updateResult = await _productFilterService.UpdateAsync(request);
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

        [HttpDelete("delete/{productId}/{filterId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteProductFilter(int productId, int filterId)
        {
            var deleteResult = await _productFilterService.DeleteAsync(productId, filterId);
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
