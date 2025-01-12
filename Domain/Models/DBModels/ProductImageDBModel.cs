using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DBModels
{
    public class ProductImageDBModel
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public virtual ProductDBModel Product { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public bool IsPrimary { get; set; }
    }
}
