namespace Domain.Models.Response.Products
{
    public class BaseProductByCategoryResponseModel
    {
        public int BaseProductId { get; set; }
        public string Title { get; set; }
        public string Brand { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<ProductGroupModifiedResponseModel> ProductGroups { get; set; }
    }
}
