using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DBModels
{
    public class PriceDBModel
    {
        [Required]
        public int ProductId { get; set; }
        public virtual ProductDBModel Product { get; set; }

        [Required]
        public int SellerId { get; set; }
        public virtual SellerDBModel Seller { get; set; }

        [Required]
        public decimal PriceValue { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }
    }
}
