using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class Price
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public int SellerId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceValue { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }
    }
}
