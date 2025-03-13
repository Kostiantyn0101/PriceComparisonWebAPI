using BLL.Services.ProductServices;
using Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Models.Request.Products;
using Domain.Models.SuccessCodes;
using Domain.Models.Exceptions;
using Domain.Models.Response.Products;

namespace PriceComparisonWebAPI.Controllers.Products
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    public class ProductCharacteristicsController : ControllerBase
    {
        private readonly IProductCharacteristicService _productCharacteristicService;
        private readonly ILogger<ProductCharacteristicsController> _logger;

        public ProductCharacteristicsController(
                IProductCharacteristicService productCharacteristicService,
                ILogger<ProductCharacteristicsController> logger)
        {
            _productCharacteristicService = productCharacteristicService;
            _logger = logger;
        }


        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductCharacteristicResponseModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> GetProductCharacteristicsByProductId(int productId)
        {
            var result = await _productCharacteristicService.GetProductCharacteristicsAsync(productId);

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


        [HttpGet("grouped/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductCharacteristicGroupResponseModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> GetGroupedProductCharacteristicsByProductId(int productId)
        {
            var result = await _productCharacteristicService.GetGroupedProductCharacteristicsAsync(productId);

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


        [HttpGet("short-grouped/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductCharacteristicGroupResponseModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> GetShortGroupedProductCharacteristicsByProductId(int productId)
        {
            var result = await _productCharacteristicService.GetShortGroupedProductCharacteristicsAsync(productId);

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


        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateProductCharacteristics([FromBody] ProductCharacteristicUpdateRequestModel request)
        {
            var result = await _productCharacteristicService.UpdateProductCharacteristicAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest, result.ErrorMessage);

            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK);
        }
    }
}
