
namespace Domain.Models.Response.Products
{
    public class PopularProductResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        public decimal MinPrice { get; set; }
    }
}
