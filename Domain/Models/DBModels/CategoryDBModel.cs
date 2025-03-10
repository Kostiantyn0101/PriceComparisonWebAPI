using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class CategoryDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? IconUrl { get; set; }
        public int? ParentCategoryId { get; set; }
        public CategoryDBModel? ParentCategory { get; set; }
        public int DisplayOrder { get; set; } // less is gigger

        public ICollection<CategoryDBModel> Subcategories { get; set; }
        public ICollection<CategoryCharacteristicDBModel> CategoryCharacteristics { get; set; }
        public ICollection<CategoryCharacteristicGroupDBModel> CategoryCharacteristicGroups { get; set; } //+
        public ICollection<RelatedCategoryDBModel> RelatedCategories { get; set; }
        public ICollection<ProductDBModel> Products { get; set; } // todo - delete
        public ICollection<BaseProductDBModel> BaseProducts { get; set; }
        public ICollection<AuctionClickRateDBModel> AuctionClickRates { get; set; }
    }
}
