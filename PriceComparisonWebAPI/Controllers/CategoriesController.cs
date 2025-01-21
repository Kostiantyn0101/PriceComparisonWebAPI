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
        private readonly ICharacteristicService _characteristicService;
        private readonly ICategoryCharacteristicService _categoryCharacteristicService;

        public CategoriesController(ILogger<CategoriesController> logger,
            ICategoryService categoryService,
            ICharacteristicService characteristicService,
            ICategoryCharacteristicService categoryCharacteristicService
            )
        {
            _logger = logger;
            _categoryService = categoryService;
            _characteristicService = characteristicService;
            _categoryCharacteristicService = categoryCharacteristicService;
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

            var result = await _categoryService.UpdateAsync(category);

            if (result.IsError)
                return new JsonResult(new { error = result.Message });

            return new JsonResult(new { message = result.Message });
        }


        [HttpDelete]
        public async Task<JsonResult> Delete([FromBody] int id)
        {
            return new JsonResult(await _categoryService.DeleteAsync(id));
        }

        [HttpGet]
        [Route("test")]
        public async Task<JsonResult> GetTest()
        {
            // Category ID to filter
            int categoryId = 3;

            // Get the category
            var categoryResponse = await _categoryService.GetFromConditionAsync(c => c.Id == categoryId);
            if (!categoryResponse.Any())
            {
                return new JsonResult(new { Message = "Category not found", IsError = true });
            }

            // Get the category-characteristic relationships for the category
            var categoryCharacteristics = await _categoryCharacteristicService
                .GetFromConditionAsync(cc => cc.CategoryId == categoryId);

            if (!categoryCharacteristics.Any())
            {
                return new JsonResult(new { Message = "No characteristics found for this category", IsError = false });
            }

            // Get the characteristics for the relationships
            var characteristicIds = categoryCharacteristics.Select(cc => cc.CharacteristicId).ToList();
            var characteristics = await _characteristicService
                .GetFromConditionAsync(c => characteristicIds.Contains(c.Id));

            // Return the result
            return new JsonResult(new
            {
                IsError = false,
                Message = "Success",
                Data = characteristics
            });

        }
    }
}
