namespace Domain.Models.Request.Products
{
    public class ProductRequestModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public string Brand { get; set; }
        public string? ModelNumber { get; set; }
        public string? GTIN { get; set; }
        public string? UPC { get; set; }
    }
}
