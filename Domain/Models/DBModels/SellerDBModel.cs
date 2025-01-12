using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DBModels
{
    public class SellerDBModel
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual UserDBModel User { get; set; }

        [Required]
        public string ApiKey { get; set; }

        public string LogoImageUrl { get; set; }

        public string WebsiteUrl { get; set; }

        [Required]
        public bool IsActive { get; set; }
        public virtual ICollection<PriceDBModel> Prices { get; set; }
        public virtual ICollection<PriceHistoryDBModel> PriceHistories { get; set; }

    }

}
