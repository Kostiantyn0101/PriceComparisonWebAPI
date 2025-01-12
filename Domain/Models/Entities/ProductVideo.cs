
using System.ComponentModel.DataAnnotations;


namespace Domain.Models.Entities
{
    public class ProductVideo
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        [MaxLength(2083)]
        public string VideoUrl { get; set; }
    }
}
