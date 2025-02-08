using AutoMapper;
using BLL.Services.CategoryCharacteristicService;
using Domain.Models.DBModels;
using Domain.Models.Exceptions;
using Domain.Models.Request.Categories;
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
            var characteristics = await _categoryCharacteristicService
               .GetQuery()
               .Where(x => x.CategoryId == categoryId)
               .Include(x => x.Characteristic)
               .ToListAsync();

            if (!characteristics.Any())
            {
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.NotFound,
                    StatusCodes.Status400BadRequest);
            }

            return new JsonResult(_mapper.Map<List<CategoryCharacteristicResponseModel>>(characteristics))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> AddCategoryCharacteristic([FromBody] CategoryCharacteristicRequestModel request)
        {
            if (request == null || request.CharacteristicIds == null || !request.CharacteristicIds.Any())
            {
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.CreateError,
                    StatusCodes.Status400BadRequest,
                    "Request must contain category ID and at least one characteristic ID.");
            }

            var result = await _categoryCharacteristicService.CreateMultipleAsync(request);

            if (result.Any(r => r.IsError))
            {
                _logger.LogError("Errors occurred during creation: {Errors}",
                                  string.Join(", ", result.Where(r => r.IsError).Select(r => r.Message)));
            }

            return GeneralApiResponseModel.GetJsonResult(
                AppSuccessCodes.CreateSuccess,
                StatusCodes.Status200OK,
                $"{result.Count(r => !r.IsError)} characteristics successfully added.");

        }

        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteCategoryCharacteristics([FromBody] CategoryCharacteristicRequestModel request)
        {
            if (request == null || request.CharacteristicIds == null || !request.CharacteristicIds.Any())
            {
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.DeleteError,
                    StatusCodes.Status400BadRequest,
                    "Request must contain category ID and at least one characteristic ID.");
            }

            var result = await _categoryCharacteristicService.DeleteMultipleAsync(request);

            if (result.Any(r => r.IsError))
            {
                _logger.LogError("Errors occurred during deletion: {Errors}",
                                  string.Join(", ", result.Where(r => r.IsError).Select(r => r.Message)));
            }

            return GeneralApiResponseModel.GetJsonResult(
                AppSuccessCodes.DeleteSuccess,
                StatusCodes.Status200OK,
                $"{result.Count(r => !r.IsError)} characteristics successfully deleted.");

        }
    }
}
