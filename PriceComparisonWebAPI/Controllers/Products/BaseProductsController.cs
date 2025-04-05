using BLL.Services.ProductServices;
using Domain.Models.Exceptions;
using Domain.Models.Request.Products;
using Domain.Models.Response.Products;
using Domain.Models.Response;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace PriceComparisonWebAPI.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
    public class BaseProductsController : ControllerBase
    {
        private readonly ILogger<BaseProductsController> _logger;
        private readonly IBaseProductService _baseProductService;


        public BaseProductsController(ILogger<BaseProductsController> logger,
            IBaseProductService baseProductService)
        {
            _logger = logger;
            _baseProductService = baseProductService;
        }

        
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseProductResponseModel))]
        public async Task<JsonResult> GetBaseProductById(int id)
        {
            var products = await _baseProductService.GetFromConditionAsync(x => x.Id == id);
            if (products == null || !products.Any())
            {
                _logger.LogError(AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }

            return new JsonResult(products.First())
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [AllowAnonymous]
        [HttpGet("onmoderation")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BaseProductResponseModel>))]
        public async Task<JsonResult> GetBaseProductsOnModeration()
        {
            var products = await _baseProductService.GetFromConditionAsync(x => x.IsUnderModeration);
            if (products == null || !products.Any())
            {
                _logger.LogError(AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }

            return new JsonResult(products)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [AllowAnonymous]
        [HttpGet("bycategory/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseProductResponseModel))]
        public async Task<JsonResult> GetBaseProductByCategoryId(int categoryId)
        {
            var products = await _baseProductService.GetFromConditionAsync(x => x.CategoryId == categoryId);
            if (products == null || !products.Any())
            {
                _logger.LogError(AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }

            return new JsonResult(products)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [Authorize(Policy = "AdminRights")]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateProduct([FromBody] BaseProductCreateRequestModel productRequest)
        {
            var result = await _baseProductService.CreateAsync(productRequest);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError,
                    StatusCodes.Status400BadRequest, result.ErrorMessage);
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK, null, result.Data);
        }

        [Authorize(Policy = "AdminRights")]
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateProduct([FromBody] BaseProductUpdateRequestModel productRequest)
        {
            var result = await _baseProductService.UpdateAsync(productRequest);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError,
                    StatusCodes.Status400BadRequest, result.ErrorMessage);
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK ,null, result.Data);
        }

        [Authorize(Policy = "AdminRights")]
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteProduct(int id)
        {
            var result = await _baseProductService.DeleteAsync(id);

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
