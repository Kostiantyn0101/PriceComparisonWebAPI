using Domain.Models.Request.Products;
using Domain.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Domain.Models.SuccessCodes;
using Domain.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using BLL.Services.ProductServices;
using Domain.Models.Response.Products;

namespace PriceComparisonWebAPI.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
    public class InstructionController : ControllerBase
    {
        private readonly IInstructionService _instructionService;
        private readonly ILogger<InstructionController> _logger;


        public InstructionController(IInstructionService instructionService, ILogger<InstructionController> logger)
        {
            _instructionService = instructionService;
            _logger = logger;
        }


        [AllowAnonymous]
        [HttpGet("{baseProductId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<InstructionResponseModel>))]
        public async Task<JsonResult> GetInstructionsByBaseProductId(int baseProductId)
        {
            var result = await _instructionService.GetFromConditionAsync(x => x.BaseProductId == baseProductId);
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
        public async Task<JsonResult> CreateInstruction([FromBody] InstructionCreateRequestModel request)
        {
            var result = await _instructionService.CreateAsync(request);
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
        public async Task<JsonResult> UpdateInstruction([FromBody] InstructionUpdateRequestModel request)
        {
            var result = await _instructionService.UpdateAsync(request);
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
        public async Task<JsonResult> DeleteInstruction(int id)
        {
            var result = await _instructionService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }
    }
}
