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
    public class SellerProductController : ControllerBase
    {
        private readonly ILogger<SellerProductController> _logger;
        private readonly ISellerProductService _sellerProductService;

        public SellerProductController(ILogger<SellerProductController> logger, ISellerProductService sellerProductService)
        {
            _logger = logger;
            _sellerProductService = sellerProductService;
        }

        [HttpPost("upload-file")]
        [Consumes("multipart/form-data")]
        //[Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UploadXmlFile()
        {
            var file = this.Request.Form.Files[0];
            if (file == null || file.Length == 0)
            {
                _logger.LogError(AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest, "File not found");
            }

            using var stream = file.OpenReadStream();
            var result = await _sellerProductService.ProcessXmlAsync(stream);

            if (result.IsSuccess)
            {
                return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.GerneralSuccess, StatusCodes.Status200OK);
            }

            return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status200OK, result.ErrorMessage);
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
