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
    public class ProductImageController : ControllerBase
    {
        private readonly ILogger<ProductImageController> _logger;
        private readonly IProductImageService _productImageService;


        public ProductImageController(ILogger<ProductImageController> logger,
            IProductImageService productImageService
            )
        {
            _logger = logger;
            _productImageService = productImageService;
        }


        [AllowAnonymous]
        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductImageResponseModel>))]
        public async Task<JsonResult> GetProductImageById(int productId)
        {
            var result = await _productImageService.GetFromConditionAsync(x => x.ProductId == productId);
            if (result == null || !result.Any())
            {
                _logger.LogError(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }

            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [AllowAnonymous]
        [HttpGet("bybaseproduct/{baseProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductImageResponseModel>))]
        public async Task<JsonResult> GetProductImageByBaseProductId(int baseProductId)
        {
            var result = await _productImageService.GetFromConditionAsync(x => x.Product.BaseProductId == baseProductId);
            if (result == null || !result.Any())
            {
                _logger.LogError(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }

            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [Authorize(Policy = "AdminRights")]
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> AddProductImages([FromForm] ProductImageCreateRequestModel model)
        {
            var result = await _productImageService.AddAsync(model);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, result.ErrorMessage, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK);
        }

        [Authorize(Policy = "AdminRights")]
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteProductImage([FromBody] ProductImageDeleteRequestModel model)
        {
            var result = await _productImageService.DeleteAsync(model);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, result.ErrorMessage, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }

        [Authorize(Policy = "AdminRights")]
        [HttpPut("setprimary")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> SetPrimaryImage([FromBody] ProductImageSetPrimaryRequestModel model)
        {
            var result = await _productImageService.SetPrimaryImageAsync(model);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, result.ErrorMessage, AppSuccessCodes.UpdateSuccess);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK);
        }
    }
}
