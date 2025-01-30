using AutoMapper;
using BLL.Services.CategoryService;
using BLL.Services.MediaServices;
using Domain.Models.Configuration;
using Domain.Models.DBModels;
using Domain.Models.Exceptions;
using Domain.Models.Response;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceComparisonWebAPI.ViewModels.Category;
using System.Runtime.InteropServices;

namespace PriceComparisonWebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public CategoriesController(ILogger<CategoriesController> logger,
            ICategoryService categoryService,
            IMapper mapper,
            IFileService fileService
            )
        {
            _logger = logger;
            _categoryService = categoryService;
            _mapper = mapper;
            _fileService = fileService;
        }

        [HttpGet("getall")]
        public async Task<JsonResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetFromConditionAsync(x => true);
                return new JsonResult(_mapper.Map<List<CategoryRequestViewModel>>(categories))
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AppErrors.General.InternalServerError);
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.InternalServerError,
                    StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetFromConditionAsync(x => x.Id == id);
                if (category == null || !category.Any())
                    return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.NotFound,
                    StatusCodes.Status404NotFound);

                return new JsonResult(_mapper.Map<CategoryRequestViewModel>(category.First()))
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AppErrors.General.InternalServerError);
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.InternalServerError,
                    StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<JsonResult> CreateCategory([FromBody] CategoryRequestViewModel categoryRequest)
        {
            if (categoryRequest == null)
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest);

            categoryRequest.ParentCategoryId =
                categoryRequest.ParentCategoryId == 0 ?
                null : categoryRequest.ParentCategoryId;

            var result = await _categoryService.CreateAsync(_mapper.Map<CategoryDBModel>(categoryRequest));

            if (result.IsError)
            {
                _logger.LogError(result.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.InternalServerError,
                    StatusCodes.Status500InternalServerError,
                    result.Exception.Message);
            }

            return GeneralApiResponseModel.GetJsonResult(
                    AppSuccessCodes.CreateSuccess,
                    StatusCodes.Status200OK);
        }

        [HttpPut]
        [Route("update")]
        public async Task<JsonResult> UpdateCategory([FromBody] CategoryRequestViewModel categoryRequest)
        {
            if (categoryRequest == null)
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest);

            categoryRequest.ParentCategoryId =
                categoryRequest.ParentCategoryId == 0 ?
                null : categoryRequest.ParentCategoryId;

            var result = await _categoryService.UpdateAsync(_mapper.Map<CategoryDBModel>(categoryRequest));

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

        [HttpDelete("delete/{id}")]
        public async Task<JsonResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (result.IsError)
            {
                _logger.LogError(result.Exception, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(
                                       AppErrors.General.InternalServerError,
                                       StatusCodes.Status500InternalServerError,
                                       result.Exception.Message);
            }

            return GeneralApiResponseModel.GetJsonResult(
                    AppSuccessCodes.DeleteSuccess,
                    StatusCodes.Status200OK);
        }

        [HttpGet("popular")]
        public async Task<JsonResult> GetPopularCategories()
        {
            try
            {
                var categories = await _categoryService.GetQuery()
                    .Take(5)
                    .ToListAsync();
                return new JsonResult(_mapper.Map<List<CategoryRequestViewModel>>(categories))
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AppErrors.General.InternalServerError);
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.InternalServerError,
                    StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPost("upload-media/{categoryId}/{mediaType}")]
        public async Task<JsonResult> UploadCategoryMedia(int categoryId, string mediaType, IFormFile file)
        {
            try
            {
                var result = await _categoryService.UploadCategoryMediaAsync(categoryId, mediaType, file);

                if (result.IsError)
                {
                    if (result.Exception is InvalidOperationException ex)
                    {
                        return GeneralApiResponseModel.GetJsonResult(
                            AppErrors.General.SizeFileError,
                            StatusCodes.Status400BadRequest, 
                            ex.Message);
                    }
                    else
                    {
                        return GeneralApiResponseModel.GetJsonResult(
                            AppErrors.General.InternalServerError,
                            StatusCodes.Status500InternalServerError,
                            result.Message);
                    }
                }

                return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, AppErrors.General.InternalServerError);
                return GeneralApiResponseModel.GetJsonResult(
                    AppErrors.General.InternalServerError,
                    StatusCodes.Status500InternalServerError,
                    ex.Message);
            }

        }
    }
}
