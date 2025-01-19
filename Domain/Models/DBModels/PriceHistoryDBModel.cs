namespace Domain.Models.DBModels
{
    public class PriceHistoryDBModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public int SellerId { get; set; }
        public SellerDBModel Seller { get; set; }

        public decimal PriceValue { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
