using AutoMapper;
using BLL.Services.CategoryService;
using Domain.Models.DBModels;
using Domain.Models.Exceptions;
using Domain.Models.Request.Categories;
using Domain.Models.Response;
using Domain.Models.Response.Categories;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RelatedCategoriesController : ControllerBase
    {
        private readonly ILogger<RelatedCategoriesController> _logger;
        private readonly IRelatedCategoryService _relatedCategoryService;
        private readonly IMapper _mapper;

        public RelatedCategoriesController(
            IRelatedCategoryService relatedCategoryService,
            ILogger<RelatedCategoriesController> logger,
            IMapper mapper
            )
        {
            _relatedCategoryService = relatedCategoryService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{categoryId}")]
        public async Task<JsonResult> GetRelatedCategories(int categoryId)
        {
            try
            {
                var relatedCategories = await _relatedCategoryService.GetFromConditionAsync(x => x.CategoryId == categoryId);
                return new JsonResult(_mapper.Map<RelatedCategoryResponseModel>(relatedCategories.First()))
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
        public async Task<JsonResult> CreateRelatedCategory([FromBody] RelatedCategoryRequestModel relatedCategory)
        {
            try
            {
                var result = await _relatedCategoryService.CreateAsync(_mapper.Map<RelatedCategoryDBModel>(relatedCategory));
                if (result.IsError)
                {
                    _logger.LogError(result.Exception, AppErrors.General.CreateError);
                    return GeneralApiResponseModel.GetJsonResult(AppErrors.General.InternalServerError,
                        StatusCodes.Status500InternalServerError, result.Exception.Message);
                }
                return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.InternalServerError,
                    StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<JsonResult> UpdateRelatedCategory([FromBody] RelatedCategoryRequestModel relatedCategory)
        {
            if (relatedCategory == null)
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest);

            var result = await _relatedCategoryService.UpdateAsync(_mapper.Map<RelatedCategoryDBModel>(relatedCategory));

            if (result.IsError)
            {
                _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(
                                    AppErrors.General.InternalServerError,
                                    StatusCodes.Status500InternalServerError,
                                    result.Exception.Message);
            }

            return GeneralApiResponseModel.GetJsonResult(
                    AppSuccessCodes.UpdateSuccess,
                    StatusCodes.Status201Created);
        }

        [HttpDelete("delete/{categoryId}/{relatedCategoryId}")]
        public async Task<JsonResult> DeleteRelatedCategory(int categoryId, int relatedCategoryId)
        {
            try
            {
                var result = await _relatedCategoryService.DeleteAsync(categoryId, relatedCategoryId);
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
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError,
                    StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
