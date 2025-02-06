using AutoMapper;
using BLL.Services.CategoryCharacteristicService;
using Domain.Models.DBModels;
using Domain.Models.Exceptions;
using Domain.Models.Response;
using Domain.Models.Response.Categories;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace PriceComparisonWebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryCharacteristicsController : ControllerBase
    {
        private readonly ILogger<CategoryCharacteristicsController> _logger;
        private readonly ICategoryCharacteristicService _categoryCharacteristicService;
        private readonly IMapper _mapper;

        public CategoryCharacteristicsController(
            ICategoryCharacteristicService categoryCharacteristicService, 
            ILogger<CategoryCharacteristicsController> logger,
            IMapper mapper
            )
        {
            _categoryCharacteristicService = categoryCharacteristicService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{categoryId}")]
        public async Task<JsonResult> GetCategoryCharacteristics(int categoryId)
        {
            try
            {
                //var characteristics = await _categoryCharacteristicService.GetFromConditionAsync(x => x.CategoryId == categoryId);

                var characteristics = await _categoryCharacteristicService
                   .GetQuery()
                   .Where(x => x.CategoryId == categoryId)
                   .Include(x => x.Characteristic) 
                   .ToListAsync();

                if (!characteristics.Any())
                {
                    return GeneralApiResponseModel.GetJsonResult(
                        AppErrors.General.NotFound,
                        StatusCodes.Status404NotFound);
                }

                return new JsonResult(_mapper.Map<List<CategoryCharacteristicResponseModel>>(characteristics))
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AppErrors.General.InternalServerError, categoryId, ex.Message);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.InternalServerError,
                    StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("add")]
        public async Task<JsonResult> AddCategoryCharacteristic(int categoryId, int characteristicId)
        {
            try
            {
                var result = await _categoryCharacteristicService.CreateAsync(new CategoryCharacteristicDBModel
                {
                    CategoryId = categoryId,
                    CharacteristicId = characteristicId
                });
                if (result.IsError)
                {
                    _logger.LogError(result.Exception, AppErrors.General.InternalServerError);
                    return GeneralApiResponseModel.GetJsonResult(AppErrors.General.InternalServerError,
                        StatusCodes.Status500InternalServerError, result.Exception.Message);
                }
                return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AppErrors.General.InternalServerError, categoryId, ex.Message);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.InternalServerError,
                    StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete/{categoryId}/{characteristicId}")]
        public async Task<JsonResult> DeleteCategoryCharacteristic(int categoryId, int characteristicId)
        {
            try
            {
                var result = await _categoryCharacteristicService.DeleteAsync(categoryId, characteristicId);

                if (result == null)
                {
                    return GeneralApiResponseModel.GetJsonResult(
                        AppErrors.General.NotFound,
                        StatusCodes.Status404NotFound);
                }

                if (result.IsError)
                {
                    _logger.LogError(result.Exception, AppErrors.General.DeleteError);
                    return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError,
                        StatusCodes.Status500InternalServerError);
                }
                return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AppErrors.General.DeleteError, categoryId, ex.Message);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError,
                    StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
