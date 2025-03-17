using BLL.Services.ProductServices;
using Domain.Models.DBModels;
using Domain.Models.Exceptions;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Categories;
using Domain.Models.Response.Products;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace PriceComparisonWebAPI.Controllers.Products
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;
        private readonly IPopularProductService _popularProductService;

        public ProductsController(ILogger<ProductsController> logger,
            IProductService productService,
            IPopularProductService popularProductService
            )
        {
            _logger = logger;
            _productService = productService;
            _popularProductService = popularProductService;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> GetProductById(int id)
        {
            var products = await _productService.GetFromConditionAsync(x => x.Id == id);
            if (products == null || !products.Any())
            {
                _logger.LogError(AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }

            var product = products.First()!;
            _ = _popularProductService.RegisterClickAsync(product.Id);

            return new JsonResult(product)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpGet("bybaseproduct{baseProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductResponseModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> GetProductsByBaseProductId(int baseProductId)
        {
            var products = await _productService.GetFromConditionAsync(x => x.BaseProductId == baseProductId);
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


        [HttpGet("empty/{id}")]
        public async Task<JsonResult> GetEmptyProductById(int id)
        {
            var products = await _productService.GetFromConditionAsync(x => x.Id == id);
            if (products == null || !products.Any())
            {
                _logger.LogError(AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }

            var product = products.First()!;
            _ = _popularProductService.RegisterClickAsync(product.Id);

            return new JsonResult(product)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateProduct([FromBody] ProductCreateRequestModel productRequest)
        {
            var result = await _productService.CreateAsync(productRequest);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK);
        }


        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateProduct([FromBody] ProductUpdateRequestModel productRequest)
        {
            var result = await _productService.UpdateAsync(productRequest);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK);
        }


        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }

        [HttpGet("popular")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PopularCategoryResponseModel>))]
        public async Task<JsonResult> GetPopularProducts()
        {
            var result = await _popularProductService.GetPopularCategoriesWithProducts();

            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [HttpGet("bycategorypaginated/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResponse<BaseProductByCategoryResponseModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> GetProductsByCategory(int categoryId, [FromQuery] int page = 1)
        {
            const int pageSize = 10;

            Expression<Func<ProductDBModel, bool>> condition = p => p.BaseProduct.CategoryId == categoryId;

            var result = await _productService.GetPaginatedProductsByCategoryAsync(categoryId, page, pageSize);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }

            return new JsonResult(result.Data)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
