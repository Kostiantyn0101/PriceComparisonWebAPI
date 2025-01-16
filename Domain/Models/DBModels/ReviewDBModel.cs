namespace Domain.Models.DBModels
{
    public class ReviewDBModel
    {
        public int Id { get; set; }
        public string ReviewUrl { get; set; }

        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

    }
}
