using System.Linq.Expressions;
using BLL.Services.SellerServices;
using Domain.Models.DBModels;
using Domain.Models.Exceptions;
using Domain.Models.Request.Seller;
using Domain.Models.Response;
using Domain.Models.Response.Seller;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers.Seller
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
    public class SellerProductDetailsController : ControllerBase
    {
        private readonly ILogger<SellerProductDetailsController> _logger;
        private readonly ISellerProductDetailsService _sellerProductDetailsService;


        public SellerProductDetailsController(ILogger<SellerProductDetailsController> logger, ISellerProductDetailsService sellerProductDetailsService)
        {
            _logger = logger;
            _sellerProductDetailsService = sellerProductDetailsService;
        }


        [AllowAnonymous]
        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SellerProductDetailsResponseModel>))]
        public async Task<JsonResult> GetSellerProductDetails(int productId)
        {
            var result = await _sellerProductDetailsService.GetSellerProductDetailsAsync(productId);

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
        [HttpGet("minmaxprices/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SellerProductPricesResponseModel))]
        public async Task<JsonResult> GetSellerProductPrices(int productId)
        {
            var result = await _sellerProductDetailsService.GetSellerProductPricesAsync(productId);

            if (!result.IsSuccess)
            {
                _logger.LogError(AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }

            return new JsonResult(result.Data)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [AllowAnonymous]
        [HttpGet("byproductgroup")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SellerProductDetailsResponseModel>))]
        public async Task<JsonResult> GetSellerProductDetailsByProductGroup([FromQuery] SellerProductDetailsRequestModel model)
        {
            var result = await _sellerProductDetailsService.GetSellerProductDetailsByProductGroupAsync(model);

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
        [HttpGet("paginated/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SellerProductDetailsResponseModel>))]
        public async Task<JsonResult> GetSellerProductDetailsPaginated(int productId, [FromQuery] int page = 1, [FromQuery] int pageSize = 3)
        {
            Expression<Func<SellerProductDetailsDBModel, bool>> condition = p => p.ProductId == productId;

            var result = await _sellerProductDetailsService.GetPaginatedSellerProductDetailsAsync(condition, page, pageSize);
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

        [Authorize(Policy = "AdminRights")]
        [HttpPost("upload-xml-file")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UploadXmlFile()
        {
            var file = Request.Form.Files[0];
            if (file == null || file.Length == 0)
            {
                _logger.LogError(AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest, "File not found");
            }

            using var stream = file.OpenReadStream();
            var result = await _sellerProductDetailsService.ProcessXmlAsync(stream);

            if (result.IsSuccess)
            {
                return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.GerneralSuccess, StatusCodes.Status200OK);
            }

            return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
        }

        [Authorize(Policy = "SellerAndAdminRights")]
        [HttpPost("upload-file")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UploadFile([FromForm] SellerProductXmlRequestModel request)
        {
            var file = request.PriceXML;
            if (file == null || file.Length == 0)
            {
                _logger.LogError(AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest, "File not found");
            }

            using var stream = file.OpenReadStream();
            var result = await _sellerProductDetailsService.ProcessXmlAsync(stream);

            if (result.IsSuccess)
            {
                return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.GerneralSuccess, StatusCodes.Status200OK);
            }

            return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
        }
    }
}
