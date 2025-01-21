using BLL.Services;
using Domain.Models.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PriceComparisonWebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<List<CategoryDBModel>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            if (categories == null || categories.Count() == 0)
            {
                return NotFound("No categories found.");
            }
            return Ok(categories);
        }
    }
}
