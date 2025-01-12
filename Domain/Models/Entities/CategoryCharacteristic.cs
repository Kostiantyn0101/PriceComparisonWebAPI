using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Entities
{
    public class CategoryCharacteristic
    {
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Required]
        public int CharacteristicId { get; set; }
        public virtual Characteristic Characteristic { get; set; }
    }
}
