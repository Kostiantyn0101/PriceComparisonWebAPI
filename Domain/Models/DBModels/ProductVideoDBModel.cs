namespace Domain.Models.DBModels
{
    public class ProductVideoDBModel
    {
        public int Id { get; set; }
        public string VideoUrl { get; set; }

        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }
    }
}
