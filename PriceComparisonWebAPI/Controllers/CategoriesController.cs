using BLL.Services;
using Domain.Models.DBModels;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoriesController(ILogger<CategoriesController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<JsonResult> GetAllCategories()
        {
            var categories = await _categoryService.GetFromConditionAsync(x => true); 
            return new JsonResult(new { categories });
        }

        [HttpPost]
        [Route("create")]
        public async Task<JsonResult> CreateCategory([FromBody] CategoryDBModel category)
        {
            if (category == null)
                return new JsonResult(new { error = "Category data is null." });

            var result = await _categoryService.CreateAsync(category);

            if (result.IsError)
                return new JsonResult(new { error = result.Message });

            return new JsonResult(new { message = result.Message });
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<JsonResult> UpdateCategory(int id, [FromBody] CategoryDBModel category)
        {
            if (category == null || category.Id != id)
                return new JsonResult(new { error = "Category data is invalid." });

            var result = await _categoryService.UpdateAsync(id, category);

            if (result.IsError)
                return new JsonResult(new { error = result.Message });

            return new JsonResult(new { message = result.Message });
        }
    }
}
