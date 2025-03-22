using BLL.Services.ProductServices;
using Domain.Models.Request.Products;
using Domain.Models.Response.Products;
using Domain.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers.Products
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
    public class ProductGroupTypeController : ControllerBase
    {
        private readonly ILogger<ProductGroupTypeController> _logger;
        private readonly IProductGroupTypeService _productGroupTypeService;


        public ProductGroupTypeController(ILogger<ProductGroupTypeController> logger, IProductGroupTypeService productGroupTypeService)
        {
            _logger = logger;
            _productGroupTypeService = productGroupTypeService;
        }

        
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductGroupTypeResponseModel>))]
        public async Task<JsonResult> GetAllProductGroupTypes()
        {
            var result = await _productGroupTypeService.GetFromConditionAsync(x => true);
            if (result == null || !result.Any())
            {
                _logger.LogError("No ProductGroupTypes found.");
                return GeneralApiResponseModel.GetJsonResult("No ProductGroupTypes found.", StatusCodes.Status400BadRequest);
            }
            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductGroupTypeResponseModel))]
        public async Task<JsonResult> GetProductGroupTypeById(int id)
        {
            var result = await _productGroupTypeService.GetFromConditionAsync(pgt => pgt.Id == id);
            if (result == null || !result.Any())
            {
                _logger.LogError("ProductGroupType not found.");
                return GeneralApiResponseModel.GetJsonResult("ProductGroupType not found.", StatusCodes.Status400BadRequest);
            }

            return new JsonResult(result.First())
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateProductGroupType([FromBody] ProductGroupTypeCreateRequestModel request)
        {
            var result = await _productGroupTypeService.CreateAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, "Error creating ProductGroupType.");
                return GeneralApiResponseModel.GetJsonResult("Error creating ProductGroupType.", StatusCodes.Status400BadRequest, result.ErrorMessage);
            }

            return GeneralApiResponseModel.GetJsonResult("ProductGroupType created successfully.", StatusCodes.Status200OK);
        }


        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateProductGroupType([FromBody] ProductGroupTypeUpdateRequestModel request)
        {
            var result = await _productGroupTypeService.UpdateAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, "Error updating ProductGroupType.");
                return GeneralApiResponseModel.GetJsonResult("Error updating ProductGroupType.", StatusCodes.Status400BadRequest, result.ErrorMessage);
            }

            return GeneralApiResponseModel.GetJsonResult("ProductGroupType updated successfully.", StatusCodes.Status200OK);
        }


        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteProductGroupType(int id)
        {
            var result = await _productGroupTypeService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, "Error deleting ProductGroupType.");
                return GeneralApiResponseModel.GetJsonResult("Error deleting ProductGroupType.", StatusCodes.Status400BadRequest, result.ErrorMessage);
            }

            return GeneralApiResponseModel.GetJsonResult("ProductGroupType deleted successfully.", StatusCodes.Status200OK);
        }
    }
}
