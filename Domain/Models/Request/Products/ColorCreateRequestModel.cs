namespace Domain.Models.Request.Products
{
    public class ColorCreateRequestModel
    {
        public string Name { get; set; }
        public string HexCode { get; set; }
        public string? GradientCode { get; set; }
    }
}
