using BLL.Services.FeedbackAndReviewServices;
using Domain.Models.Request.Feedback;
using Domain.Models.Response;
using Domain.Models.Response.Feedback;
using Domain.Models.SuccessCodes;
using Domain.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers.Products
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
    public class FeedbackImageController : ControllerBase
    {
        private readonly IFeedbackImageService _feedbackImageService;
        private readonly ILogger<FeedbackImageController> _logger;

        public FeedbackImageController(IFeedbackImageService feedbackImageService, ILogger<FeedbackImageController> logger)
        {
            _feedbackImageService = feedbackImageService;
            _logger = logger;
        }


        [HttpGet("{feedbackId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FeedbackImageResponseModel>))]
        public async Task<JsonResult> GetImagesByFeedbackId(int feedbackId)
        {   
            var result = await _feedbackImageService.GetFromConditionAsync(i => i.FeedbackId == feedbackId);
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


        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> AddFeedbackImages([FromForm] FeedbackImageCreateRequestModel request)
        {
            var result = await _feedbackImageService.CreateAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK);
        }


        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteFeedbackImages([FromBody] FeedbackImageDeleteRequestModel request)
        {
            var result = await _feedbackImageService.DeleteAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }
    }
}
