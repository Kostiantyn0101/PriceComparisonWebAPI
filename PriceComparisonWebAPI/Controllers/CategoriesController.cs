using BLL.Services;
using Domain.Models.DBModels;
using Domain.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PriceComparisonWebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoriesController(ILogger<CategoriesController> logger,
            ICategoryService categoryService
            )
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllCategorie()
        {
            try
            {
                var categories = await _categoryService.GetFromConditionAsync(x => true);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetFromConditionAsync(x => x.Id == id);
                if (category == null || !category.Any())
                    return NotFound(new { error = "Category not found" });

                return Ok(category.First());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching category by ID");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequestModel categoryRequest)
        {
            if (categoryRequest == null)
                return BadRequest(new { error = "Category data is null" });

            var category = new CategoryDBModel
            {
                Title = categoryRequest.Title,
                ImageUrl = categoryRequest.ImageUrl,
                IconUrl = categoryRequest.IconUrl,
                ParentCategoryId = categoryRequest.ParentCategoryId
            };

            var result = await _categoryService.CreateAsync(category);

            if (result.IsError)
            {
                _logger.LogError(result.Exception, "Error creating category");
                return BadRequest(new { error = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequestModel categoryRequest)
        {
            if (categoryRequest == null)
                return BadRequest(new { error = "Invalid category data" });

            var category = new CategoryDBModel
            {
                Id = id,
                Title = categoryRequest.Title,
                ImageUrl = categoryRequest.ImageUrl,
                IconUrl = categoryRequest.IconUrl,
                ParentCategoryId = categoryRequest.ParentCategoryId
            };

            var result = await _categoryService.UpdateAsync(category);

            if (result.IsError)
            {
                _logger.LogError(result.Exception, "Error updating category");
                return BadRequest(new { error = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (result.IsError)
            {
                _logger.LogError(result.Exception, "Error deleting category");
                return BadRequest(new { error = result.Message });
            }

            return NoContent();
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularCategories()
        {
            try
            {
                var categories = await _categoryService.GetQuery()
                    .Take(5)
                    .ToListAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching popular categories");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
