using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DBModels
{
    public class PriceHistoryDBModel
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public virtual ProductDBModel Product { get; set; }

        [Required]
        public int SellerId { get; set; }
        public virtual SellerDBModel Seller { get; set; }

        [Required]
        public decimal PriceValue { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }

}
