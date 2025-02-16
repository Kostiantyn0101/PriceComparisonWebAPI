using Microsoft.AspNetCore.Http;

namespace Domain.Models.Request.Categories
{
    public class CategoryCreateRequestModel
    {
        public string Title { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFile? Icon { get; set; }
        public int? ParentCategoryId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
