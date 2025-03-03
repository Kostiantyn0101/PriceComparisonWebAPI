using System.Xml.Linq;
using BLL.Services.SellerServices;
using Domain.Models.Exceptions;
using Domain.Models.Response;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers.Seller
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    public class SellerProductDetailsController : ControllerBase
    {
        private readonly ILogger<SellerProductDetailsController> _logger;
        private readonly ISellerProductDetailsService _sellerProductDetailsService;

        public SellerProductDetailsController(ILogger<SellerProductDetailsController> logger, ISellerProductDetailsService sellerProductDetailsService)
        {
            _logger = logger;
            _sellerProductDetailsService = sellerProductDetailsService;
        }

        [HttpPost("upload-file")]
        [Consumes("multipart/form-data")]
        //[Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
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

            return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status200OK, result.ErrorMessage);
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
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


        //[HttpPost("upload-xml")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        //public async Task<IActionResult> UploadXml([FromBody] string xmlContent)
        //{
        //    //await _sellerProductService.ProcessXmlAsync(xmlContent);
        //    //return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.GerneralSuccess, StatusCodes.Status200OK);
        //}
    }
}
