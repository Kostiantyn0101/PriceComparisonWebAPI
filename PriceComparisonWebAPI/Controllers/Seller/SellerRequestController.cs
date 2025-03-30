using BLL.Services.SellerServices;
using Domain.Models.Exceptions;
using Domain.Models.Request.Seller;
using Domain.Models.Response.Seller;
using Domain.Models.Response;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers.Seller
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
    public class SellerRequestController : ControllerBase
    {
        private readonly ILogger<SellerRequestController> _logger;
        private readonly ISellerRequestService _sellerRequestService;


        public SellerRequestController(ILogger<SellerRequestController> logger, ISellerRequestService sellerRequestService)
        {
            _logger = logger;
            _sellerRequestService = sellerRequestService;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SellerRequestResponseModel))]
        public async Task<JsonResult> GetSellerRequestById(int id)
        {
            var result = await _sellerRequestService.GetFromConditionAsync(s => s.Id == id);
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

        [HttpGet("getByUserId/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SellerRequestResponseModel>))]
        public async Task<JsonResult> GetSellerRequestByUserId(int userId)
        {
            var result = await _sellerRequestService.GetFromConditionAsync(s => s.UserId == userId);
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

        [HttpGet("getAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SellerRequestResponseModel>))]
        public async Task<JsonResult> GetAllSellerRequests()
        {
            var result = (await _sellerRequestService.GetFromConditionAsync(s => true))
                .OrderBy(r => r.CreatedAt);
            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [HttpGet("getPending")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SellerRequestResponseModel>))]
        public async Task<JsonResult> GetPendingSellerRequests()
        {
            var result = (await _sellerRequestService.GetFromConditionAsync(s => !s.IsProcessed))
                .OrderBy(r => r.CreatedAt); 
            return new JsonResult(result)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateSellerRequest([FromBody] SellerRequestCreateRequestModel request)
        {
            var result = await _sellerRequestService.CreateAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateSellerRequest([FromBody] SellerRequestUpdateRequestModel request)
        {
            var result = await _sellerRequestService.UpdateAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK);
        }

        [HttpPut("process")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> ProcessSellerRequest([FromBody] SellerRequestProcessRequestModel request)
        {
            var result = await _sellerRequestService.ProcessRequestAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK);
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteSellerRequest(int id)
        {
            var result = await _sellerRequestService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }
    }
}
