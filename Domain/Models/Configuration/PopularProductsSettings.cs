namespace Domain.Models.Configuration
{
    public class PopularProductsSettings
    {
        public const string Position = "PopularProducts";
        public int LookbackMonths { get; set; }
        public int PopularCategoriesCount { get; set; }
        public int PopularProductsPerCategoryCount { get; set; }
    }
}
