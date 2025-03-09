using BLL.Services.FilterServices; 
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetFiltersByProduct(int productId)
        {
            var filters = await _productFilterByCharacteristicService.GetFiltersByProductIdAsync(productId);
            if (filters == null || !filters.Any())
            {
                _logger.LogError("No filters found for product id {ProductId}", productId);
                return NotFound("No filters found for the specified product.");
            }
            return Ok(filters);
        }

        [HttpGet("productsByFilter/{filterId}")]
        public async Task<IActionResult> GetProductsByFilter(int filterId)
        {
            var products = await _productFilterByCharacteristicService.GetProductsByFilterIdAsync(filterId);
            if (products == null || !products.Any())
            {
                _logger.LogError("No products found for filter id {FilterId}", filterId);
                return NotFound("No products found for the specified filter.");
            }
            return Ok(products);
        }
    }
}
