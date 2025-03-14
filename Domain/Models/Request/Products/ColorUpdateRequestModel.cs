namespace Domain.Models.Request.Products
{
    public class ColorUpdateRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HexCode { get; set; }
        public string? GradientCode { get; set; }
    }
}
