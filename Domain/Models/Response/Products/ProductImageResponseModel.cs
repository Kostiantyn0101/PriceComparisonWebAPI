namespace Domain.Models.Response.Products
{
    public class ProductImageResponseModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; }
    }
}
