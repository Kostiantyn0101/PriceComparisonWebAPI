namespace Domain.Models.Response.Products
{
    public class ProductSearchResponseModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? ImageUrl { get; set; }
        public decimal? MinPrice { get; set; }
    }
}
