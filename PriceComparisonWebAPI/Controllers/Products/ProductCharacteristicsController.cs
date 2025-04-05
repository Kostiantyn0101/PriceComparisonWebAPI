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
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
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


        [AllowAnonymous]
        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductCharacteristicResponseModel>))]
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

        [AllowAnonymous]
        [HttpGet("baseproduct/{baseProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductCharacteristicResponseModel>))]
        public async Task<JsonResult> GetProductCharacteristicsByBaseProductId(int baseProductId)
        {
            var result = await _productCharacteristicService.GetBaseProductCharacteristicsAsync(baseProductId);

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
        [HttpGet("grouped/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductCharacteristicGroupResponseModel>))]
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

        [AllowAnonymous]
        [HttpGet("short-grouped/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductCharacteristicGroupResponseModel>))]
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

        [Authorize(Policy = "AdminRights")]
        [HttpPost("createlist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateProductCharacteristicList([FromBody] IEnumerable<ProductCharacteristicCreateRequestModel> request)
        {
            var result = await _productCharacteristicService.CreateProductCharacteristicsAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, result.ErrorMessage);

            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK,null, result.Data);
        }

        [Authorize(Policy = "AdminRights")]
        [HttpPut("updatelist")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateProductCharacteristicList([FromBody] IEnumerable<ProductCharacteristicUpdateRequestModel> request)
        {
            var result = await _productCharacteristicService.UpdateProductCharacteristicsAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest, result.ErrorMessage);

            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK, null, result.Data);
        }

        [Authorize(Policy = "AdminRights")]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateProductCharacteristic([FromBody] ProductCharacteristicCreateRequestModel request)
        {
            var result = await _productCharacteristicService.CreateAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, result.ErrorMessage);

            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK, null, result.Data);
        }

        [Authorize(Policy = "AdminRights")]
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateProductCharacteristic([FromBody] ProductCharacteristicUpdateRequestModel request)
        {
            var result = await _productCharacteristicService.UpdateAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest, result.ErrorMessage);

            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK, null, result.Data);
        }

        [Authorize(Policy = "AdminRights")]
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteProductCharacteristics(int id)
        {
            var result = await _productCharacteristicService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError, StatusCodes.Status400BadRequest, result.ErrorMessage);

            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }
    }
}

