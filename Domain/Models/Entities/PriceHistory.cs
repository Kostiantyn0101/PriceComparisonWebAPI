using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Entities
{
    public class PriceHistory
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public int SellerId { get; set; }
        public virtual Seller Seller { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceValue { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }

}
