namespace PriceComparisonWebAPI.ViewModels.Category
{
    public class CategoryResponseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? IconUrl { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
