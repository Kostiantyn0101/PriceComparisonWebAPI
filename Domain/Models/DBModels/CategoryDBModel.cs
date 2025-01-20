using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class CategoryDBModel : EntityDBModel
    {
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        public int? ParentCategoryId { get; set; }
        public CategoryDBModel? ParentCategory { get; set; }

        public ICollection<CategoryDBModel> Subcategories { get; set; }
        public ICollection<CategoryCharacteristicDBModel> CategoryCharacteristics { get; set; }
        public ICollection<RelatedCategoryDBModel> RelatedCategories { get; set; }
        public ICollection<ProductDBModel> Products { get; set; }
    }
}
