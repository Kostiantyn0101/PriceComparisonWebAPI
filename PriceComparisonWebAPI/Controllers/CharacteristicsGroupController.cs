using BLL.Services.ProductServices;
using Domain.Models.Exceptions;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
    public class CharacteristicGroupsController : ControllerBase
    {
        private readonly ILogger<CharacteristicGroupsController> _logger;
        private readonly ICharacteristicGroupService _characteristicGroupService;


        public CharacteristicGroupsController(ICharacteristicGroupService characteristicGroupService,
            ILogger<CharacteristicGroupsController> logger
            )
        {
            _characteristicGroupService = characteristicGroupService;
            _logger = logger;
        }


        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CharacteristicGroupResponseModel))]
        public async Task<JsonResult> GetCharacteristicGroupById(int id)
        {
            var characteristicGroup = (await _characteristicGroupService.GetFromConditionAsync(x => x.Id == id)).FirstOrDefault();
            if (characteristicGroup == null)
            {
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }
            return new JsonResult(characteristicGroup)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [AllowAnonymous]
        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CharacteristicGroupResponseModel>))]
        public async Task<JsonResult> GetAllCharacteristicGroups()
        {
            var characteristicGroups = await _characteristicGroupService.GetFromConditionAsync(x => true);
            if (characteristicGroups == null)
            {
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }
            return new JsonResult(characteristicGroups)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [Authorize(Policy = "AdminRights")]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateCharacteristicGroup([FromBody] CharacteristicGroupCreateRequestModel model)
        {
            var result = await _characteristicGroupService.CreateAsync(model);
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
        public async Task<JsonResult> UpdateCharacteristicGroup([FromBody] CharacteristicGroupRequestModel model)
        {
            var result = await _characteristicGroupService.UpdateAsync(model);
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
        public async Task<JsonResult> DeleteCharacteristicGroup(int id)
        {
            var result = await _characteristicGroupService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError, StatusCodes.Status400BadRequest, result.ErrorMessage);
            }
            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }
    }
}