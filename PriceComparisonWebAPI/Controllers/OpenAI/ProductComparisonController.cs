using BLL.Services.AIServices;
using Domain.Models.Exceptions;
using Domain.Models.Response;
using Domain.Models.Response.Gpt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers.OpenAI
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
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


        [AllowAnonymous]
        [HttpGet("comparegpt")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AIComparisonProductCharacteristicResponseModel))]
        public async Task<JsonResult> CompareOpenAIProducts([FromQuery] int productIdA, [FromQuery] int productIdB)
        {
            if (productIdA <= 0 || productIdB <= 0)
            {
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest, "Invalid product IDs.");
            }

            var result = await _productComparisonService.CompareProductsAsync(productIdA, productIdB, AIProvider.OpenAI);
            if (result == null)
            {
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest, "Products not found.");
            }

            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [AllowAnonymous]
        [HttpGet("compareclaude")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AIComparisonProductCharacteristicResponseModel))]
        public async Task<JsonResult> CompareClaudeProducts([FromQuery] int productIdA, [FromQuery] int productIdB)
        {
            if (productIdA <= 0 || productIdB <= 0)
            {
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest, "Invalid product IDs.");
            }

            var result = await _productComparisonService.CompareProductsAsync(productIdA, productIdB, AIProvider.Claude);
            if (result == null)
            {
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest, "Products not found.");
            }

            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
