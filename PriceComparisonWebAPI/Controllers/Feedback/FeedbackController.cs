using BLL.Services.FeedbackAndReviewServices;
using Domain.Models.DBModels;
using Domain.Models.Exceptions;
using Domain.Models.Request.Feedback;
using Domain.Models.Response;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace PriceComparisonWebAPI.Controllers.Products
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ILogger<FeedbackController> _logger;

        public FeedbackController(IFeedbackService feedbackService, ILogger<FeedbackController> logger)
        {
            _feedbackService = feedbackService;
            _logger = logger;
        }

        [HttpGet("{baseProductId}")]
        public async Task<JsonResult> GetFeedbacksByBaseProductId(int baseProductId)
        {
            var result = await _feedbackService.GetFromConditionAsync(x => x.BaseProductId == baseProductId);
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

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateFeedback([FromBody] FeedbackCreateRequestModel request)
        {
            var result = await _feedbackService.CreateAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateFeedback([FromBody] FeedbackUpdateRequestModel request)
        {
            var result = await _feedbackService.UpdateAsync(request);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK);
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteFeedback(int id)
        {
            var result = await _feedbackService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }

        [HttpGet("paginated/{baseProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> GetFeedbacksByBaseProductIdPaginated(int baseProductId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            Expression<Func<FeedbackDBModel, bool>> condition = f => f.BaseProductId == baseProductId;
            var result = await _feedbackService.GetPaginatedFeedbacksAsync(condition, page, pageSize);

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

    }
}
