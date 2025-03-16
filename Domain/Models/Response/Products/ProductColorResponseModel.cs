namespace Domain.Models.Response.Products
{
    public class ProductColorResponseModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string HexCode { get; set; }
        public string? GradientCode { get; set; }
    }
}
