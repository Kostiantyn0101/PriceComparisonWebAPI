namespace Domain.Models.Request.Products
{
    public class ProductVideoUpdateRequestModel
    {
        public int Id { get; set; }
        public int BaseProductId { get; set; }
        public string VideoUrl { get; set; }
    }
}
