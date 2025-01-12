using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Entities
{
    public class RelatedCategory
    {
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Required]
        public int RelatedCategoryId { get; set; }

        [ForeignKey("RelatedCategoryId")]
        public virtual Category RelatedCategoryItem { get; set; }
    }
}
