namespace Domain.Models.Request.Products
{
    public class ProductRequestModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
    }
}
