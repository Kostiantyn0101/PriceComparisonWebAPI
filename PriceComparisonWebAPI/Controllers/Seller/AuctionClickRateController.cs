using BLL.Services.SellerServices;
using Domain.Models.Exceptions;
using Domain.Models.Request.Seller;
using Domain.Models.Response;
using Domain.Models.Response.Seller;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers.Seller
{
    [Authorize(Policy = "AdminRights")]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
    public class AuctionClickRateController : ControllerBase
    {
        private readonly ILogger<AuctionClickRateController> _logger;
        private readonly IAuctionClickRateService _auctionClickRateService;


        public AuctionClickRateController(ILogger<AuctionClickRateController> logger, IAuctionClickRateService auctionClickRateService)
        {
            _logger = logger;
            _auctionClickRateService = auctionClickRateService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuctionClickRateResponseModel))]
        public async Task<JsonResult> GetAuctionClickRateById(int id)
        {
            var result = await _auctionClickRateService.GetFromConditionAsync(a => a.Id == id);
            if (result == null || !result.Any())
            {
                _logger.LogError(AppErrors.General.NotFound);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }
            return new JsonResult(result.First())
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [AllowAnonymous]
        [HttpGet("getBySellerId/{sellerId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AuctionClickRateResponseModel>))]
        public async Task<JsonResult> GetAuctionClickRateBySellerId(int sellerId)
        {
            var result = await _auctionClickRateService.GetFromConditionAsync(a => a.SellerId == sellerId);
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

        [Authorize(Policy = "AdminRights")]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateAuctionClickRate([FromBody] AuctionClickRateCreateRequestModel request)
        {
            var result = await _auctionClickRateService.CreateAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK);
        }

        [Authorize(Policy = "AdminRights")]
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateAuctionClickRate([FromBody] AuctionClickRateUpdateRequestModel request)
        {
            var result = await _auctionClickRateService.UpdateAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK);
        }

        [Authorize(Policy = "AdminRights")]
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteAuctionClickRate(int id)
        {
            var result = await _auctionClickRateService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }
    }
}
