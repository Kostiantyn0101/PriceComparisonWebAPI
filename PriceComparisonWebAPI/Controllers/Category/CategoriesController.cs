using AutoMapper;
using BLL.Services.CategoryService;
using Domain.Models.Exceptions;
using Domain.Models.Request.Categories;
using Domain.Models.Response;
using Domain.Models.Response.Categories;
using Domain.Models.SuccessCodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PriceComparisonWebAPI.Controllers.Category
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GeneralApiResponseModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GeneralApiResponseModel))]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ILogger<CategoriesController> logger,
            ICategoryService categoryService,
            IMapper mapper
            )
        {
            _logger = logger;
            _categoryService = categoryService;
            _mapper = mapper;
        }


        [HttpGet("getallparents")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryResponseModel>))]
        public async Task<JsonResult> GetAllParentCategories()
        {
            var categories = await _categoryService.GetFromConditionAsync(x => x.IsParent);
            return new JsonResult(categories)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpGet("getbyparent/{parentCategoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryResponseModel>))]
        public async Task<JsonResult> GetCategoryByParent(int parentCategoryId)
        {
            var categories = await _categoryService.GetFromConditionAsync(x => x.ParentCategoryId == parentCategoryId);
            return new JsonResult(categories)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpGet("getbyproduct/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryResponseModel))]
        public async Task<JsonResult> GetCategoryByProduct(int productId)
        {
            var category = await _categoryService.GetQuery()
                 .Where(c => c.BaseProducts.Any(bp => bp.Products.Any(p => p.Id == productId)))
                 .FirstOrDefaultAsync();

            if (category == null)
            {
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }

            return new JsonResult(category)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryResponseModel>))]
        public async Task<JsonResult> GetAllCategories()
        {
            var categories = await _categoryService.GetFromConditionAsync(x => true);
            return new JsonResult(categories)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryResponseModel))]
        public async Task<JsonResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetFromConditionAsync(x => x.Id == id);
            if (category == null || !category.Any())
            {
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.NotFound, StatusCodes.Status400BadRequest);
            }

            return new JsonResult(_mapper.Map<CategoryResponseModel>(category.First()))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }


        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> CreateCategory([FromBody] CategoryCreateRequestModel categoryRequest)
        {
            var result = await _categoryService.CreateAsync(categoryRequest);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.CreateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.CreateError, StatusCodes.Status400BadRequest, result.Exception?.Message);
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK, null, result.Data);
        }


        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> UpdateCategory([FromBody] CategoryUpdateRequestModel categoryRequest)
        {
            var result = await _categoryService.UpdateAsync(categoryRequest);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.UpdateError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.UpdateError, StatusCodes.Status400BadRequest, result.ErrorMessage!);
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.CreateSuccess, StatusCodes.Status200OK, null, result.Data);
        }


        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GeneralApiResponseModel))]
        public async Task<JsonResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, AppErrors.General.DeleteError);
                return GeneralApiResponseModel.GetJsonResult(AppErrors.General.DeleteError, StatusCodes.Status400BadRequest, result.ErrorMessage!);
            }

            return GeneralApiResponseModel.GetJsonResult(AppSuccessCodes.DeleteSuccess, StatusCodes.Status200OK);
        }


        [HttpGet("popular")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryResponseModel>))]
        public async Task<JsonResult> GetPopularCategories()
        {
            var categories = await _categoryService.GetQuery()
                .Take(5)
                .ToListAsync();
            return new JsonResult(_mapper.Map<List<CategoryResponseModel>>(categories))
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
