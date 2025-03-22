using BLL.Services.FilterServices;
using Domain.Models.Exceptions;
using Domain.Models.Response;
using Domain.Models.Response.Filters;
using Domain.Models.Response.Products;
using Microsoft.AspNetCore.Mvc;
using static NuGet.Packaging.PackagingConstants;

namespace PriceComparisonWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
    public class FilterEvaluationController : ControllerBase
    {
        private readonly ILogger<FilterEvaluationController> _logger;
        private readonly IProductFilterByCharacteristicService _productFilterByCharacteristicService;


        public FilterEvaluationController(ILogger<FilterEvaluationController> logger, 
            IProductFilterByCharacteristicService productFilterByCharacteristicService)
        {
            _logger = logger;
            _productFilterByCharacteristicService = productFilterByCharacteristicService;
        }


        [HttpGet("filtersByProduct/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FilterResponseModel>))]
        public async Task<JsonResult> GetFiltersByProduct(int productId)
        {
            var filters = await _productFilterByCharacteristicService.GetFiltersByProductIdAsync(productId);
            if (!filters.IsSuccess)
            {
                _logger.LogError("No filters found for product id {ProductId}", productId);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }
            return new JsonResult(filters)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpGet("productsByFilter/{filterId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FilterResponseModel>))]
        public async Task<JsonResult> GetProductsByFilter(int filterId)
        {
            var products = await _productFilterByCharacteristicService.GetProductsByFilterIdAsync(filterId);
            if (!products.IsSuccess)
            {
                _logger.LogError("No products found for filter id {FilterId}", filterId);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }
            return new JsonResult(products)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpGet("productsByFilters")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductResponseModel>))]
        public async Task<JsonResult> GetProductsByFilters([FromQuery] int[] filterIds)
        {
            var products = await _productFilterByCharacteristicService.GetProductsByFilterIdsAsync(filterIds);
            if (!products.IsSuccess)
            {
                _logger.LogError("No products found for filters: {filterIds}", filterIds);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }
            return new JsonResult(products)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpGet("filtersByCategory/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FilterResponseModel>))]
        public async Task<JsonResult> GetFiltersByCategory(int categoryId)
        {
            var filters = await _productFilterByCharacteristicService.GetFiltersByCategoryIdAsync(categoryId);
            if (!filters.IsSuccess)
            {
                _logger.LogError("No filters found for category id {CategoryId}", categoryId);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }
            return new JsonResult(filters)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
