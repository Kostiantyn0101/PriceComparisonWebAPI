namespace Domain.Models.Request.Products
{
    public class ReviewUpdateRequestModel
    {
        public int Id { get; set; }
        public int BaseProductId { get; set; }
        public string ReviewUrl { get; set; }
    }
}
