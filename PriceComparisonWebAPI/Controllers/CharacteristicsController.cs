using BLL.Services.ProductServices;
using Domain.Models.Exceptions;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
    public class CharacteristicsController : ControllerBase
    {
        private readonly ILogger<CharacteristicsController> _logger;
        private readonly ICharacteristicService _characteristicService;


        public CharacteristicsController(ICharacteristicService characteristicService,
            ILogger<CharacteristicsController> logger
            )
        {
            _characteristicService = characteristicService;
            _logger = logger;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CharacteristicResponseModel))]
        public async Task<JsonResult> GetCharacteristicById(int id)
        {
            var characteristic = (await _characteristicService.GetFromConditionAsync(x => x.Id == id)).FirstOrDefault();
            if (characteristic == null)
            {
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.NotFound,
                    StatusCodes.Status400BadRequest);
            }
            return new JsonResult(characteristic)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CharacteristicResponseModel))]
        public async Task<JsonResult> GetAllCharacteristics()
        {
            var characteristic = await _characteristicService.GetFromConditionAsync(x => true);
            if (characteristic == null)
            {
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }
            return new JsonResult(characteristic)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateCharacteristic([FromBody] CharacteristicCreateRequestModel model)
        {
            var result = await _characteristicService.CreateAsync(model);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK);
        }


        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateCharacteristic([FromBody] CharacteristicRequestModel model)
        {
            var result = await _characteristicService.UpdateAsync(model);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError,
                    StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK);
        }


        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteCharacteristic(int id)
        {
            var result = await _characteristicService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError,
                    StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }
    }
}