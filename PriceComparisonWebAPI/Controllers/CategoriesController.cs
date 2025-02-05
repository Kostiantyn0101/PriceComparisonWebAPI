using AutoMapper;
using BLL.Services.CategoryService;
using BLL.Services.MediaServices;
using Domain.Models.Configuration;
using Domain.Models.DBModels;
using Domain.Models.DTO.Categories;
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
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
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
            var categories = await _categoryService.GetFromConditionAsync(x => true);
            return new JsonResult(_mapper.Map<List<CategoryRequestViewModel>>(categories))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> GetCategoryById(int id)
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

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateCategory([FromForm] CategoryCreateRequestModel categoryRequest)
        {
            var result = await _categoryService.CreateAsync(categoryRequest);

            if (result.IsError)
            {
                _logger.LogError(result.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, result.Exception.Message);
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateCategory([FromForm] CategoryUpdateRequestModel categoryRequest)
        {
            var result = await _categoryService.UpdateAsync(categoryRequest);

            if (result.IsError)
            {
                _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(
                                    AppErrors.General.UpdateError,
                                    StatusCodes.Status400BadRequest,
                                    result.Message);
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.UpdateSuccess, StatusCodes.Status200OK);
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
            var categories = await _categoryService.GetQuery()
                .Take(5)
                .ToListAsync();
            return new JsonResult(_mapper.Map<List<CategoryRequestViewModel>>(categories))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
    } 
}
