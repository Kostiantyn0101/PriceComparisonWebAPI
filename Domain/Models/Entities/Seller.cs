using System.ComponentModel.DataAnnotations;


namespace Domain.Models.Entities
{
    public class Seller
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        [MaxLength(255)]
        public string ApiKey { get; set; }

        [MaxLength(2083)]
        public string LogoImageUrl { get; set; }

        [MaxLength(2083)]
        public string WebsiteUrl { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }

}
