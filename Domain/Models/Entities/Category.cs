using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(2083)]
        public string ImageUrl { get; set; }

        public int? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        public virtual Category ParentCategory { get; set; }

        public virtual ICollection<Category> Subcategories { get; set; }
        public virtual ICollection<CategoryCharacteristic> CategoryCharacteristics { get; set; }
        public virtual ICollection<RelatedCategory> RelatedCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
