using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Entities
{
    public class Characteristic
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [MaxLength(50)]
        public string DataType { get; set; }

        [MaxLength(50)]
        public string Unit { get; set; }

        public virtual ICollection<CategoryCharacteristic> CategoryCharacteristics { get; set; }
        public virtual ICollection<ProductCharacteristic> ProductCharacteristics { get; set; }
    }
}
