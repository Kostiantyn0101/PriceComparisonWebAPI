using AutoMapper;
using BLL.Services.ProductServices;
using Domain.Models.DBModels;
using Domain.Models.Exceptions;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CharacteristicsController : ControllerBase
    {
        private readonly ILogger<CharacteristicsController> _logger;
        private readonly ICharacteristicService _characteristicService;
        private readonly IMapper _mapper;

        public CharacteristicsController(ICharacteristicService characteristicService, 
            ILogger<CharacteristicsController> logger,
            IMapper mapper
            )
        {
            _characteristicService = characteristicService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> GetCharacteristicById(int id)
        {
            try
            {
                var characteristic = await _characteristicService.GetFromConditionAsync(x => x.Id == id);
                if (!characteristic.Any())
                    return GeneralApiResponseModel.GetJsonResult(
                        AppErrors.General.NotFound, 
                        StatusCodes.Status404NotFound);

                return new JsonResult(_mapper.Map<CharacteristicResponseModel>(characteristic.First()))
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AppErrors.General.InternalServerError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.InternalServerError,
                    StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateCharacteristic([FromBody] CharacteristicRequestModel model)
        {
            try
            {
                var result = await _characteristicService.CreateAsync(_mapper.Map<CharacteristicDBModel>(model));
                if (result.IsError)
                {
                    _logger.LogError(result.Exception, AppErrors.General.CreateError);
                    return GeneralApiResponseModel.GetJsonResult(AppErrors.General.InternalServerError,
                        StatusCodes.Status500InternalServerError, result.Exception.Message);
                }
                return GeneralApiResponseModel.GetJsonResult(
                    AppSuccessCodes.CreateSuccess, 
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.InternalServerError,
                    StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateCharacteristic([FromBody] CharacteristicRequestModel model)
        {
            try
            {
                var result = await _characteristicService.UpdateAsync(_mapper.Map<CharacteristicDBModel>(model));
                if (result.IsError)
                {
                    _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                    return GeneralApiResponseModel.GetJsonResult(AppErrors.General.InternalServerError,
                        StatusCodes.Status500InternalServerError, result.Exception.Message);
                }
                return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.InternalServerError,
                    StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteCharacteristic(int id)
        {
            try
            {
                var result = await _characteristicService.DeleteAsync(id);
                if (result.IsError)
                {
                    _logger.LogError(result.Exception, AppErrors.General.DeleteError);
                    return GeneralApiResponseModel.GetJsonResult(AppErrors.General.InternalServerError,
                        StatusCodes.Status500InternalServerError, result.Exception.Message);
                }
                return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.InternalServerError,
                    StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}