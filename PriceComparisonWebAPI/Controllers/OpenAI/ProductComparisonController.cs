using BLL.Services.ProductServices;
using Domain.Models.Exceptions;
using Domain.Models.Response;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CompareProducts([FromQuery] int productIdA, [FromQuery] int productIdB)
        {
            if (productIdA <= 0 || productIdB <= 0)
            {
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.NotFound,
                    StatusCodes.Status400BadRequest,
                    "Invalid product IDs.");
            }

            var result = await _productComparisonService.CompareProductsAsync(productIdA, productIdB);
            if (result == null)
            {
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.NotFound,
                    StatusCodes.Status400BadRequest,
                    "Products not found.");
            }

            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
