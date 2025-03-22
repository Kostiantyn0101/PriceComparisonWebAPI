namespace Domain.Models.Request.Products
{
    public class ReviewCreateRequestModel
    {
        public int BaseProductId { get; set; }
        public string ReviewUrl { get; set; }
    }
}
