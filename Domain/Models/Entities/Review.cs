using System.ComponentModel.DataAnnotations;


namespace Domain.Models.Entities
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        [MaxLength(2083)]
        public string ReviewUrl { get; set; }
    }
}
