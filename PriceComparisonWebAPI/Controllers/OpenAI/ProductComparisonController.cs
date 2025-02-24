using BLL.Services.ProductServices;
using Domain.Models.Response.Gpt;
using Domain.Models.Response.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers.OpenAI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductComparisonController : ControllerBase
    {
        private readonly IProductComparisonService _productComparisonService;
        private readonly ILogger<ProductComparisonController> _logger;

        public ProductComparisonController(IProductComparisonService productComparisonService,
                                           ILogger<ProductComparisonController> logger)
        {
            _productComparisonService = productComparisonService;
            _logger = logger;
        }

        [HttpGet("compare")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GptComparisonResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CompareProducts([FromQuery] int productIdA, [FromQuery] int productIdB)
        {
            if (productIdA <= 0 || productIdB <= 0)
            {
                return BadRequest("Invalid product IDs.");
            }

            try
            {
                var (productA, productB, explanation) = await _productComparisonService.CompareProductsAsync(productIdA, productIdB);

                var response = new GptComparisonResponse
                {
                    ProductA = productA,
                    ProductB = productB,
                    Explanation = explanation
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while comparing products");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while comparing products.");
            }

        }
    }
}
