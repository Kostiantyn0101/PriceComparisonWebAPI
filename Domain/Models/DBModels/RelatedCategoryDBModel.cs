using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DBModels
{
    public class RelatedCategoryDBModel
    {
        [Required]
        public int CategoryId { get; set; }
        public virtual CategoryDBModel Category { get; set; }

        [Required]
        public int RelatedCategoryId { get; set; }
        public virtual CategoryDBModel RelatedCategoryItem { get; set; }
    }
}
