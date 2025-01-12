using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DBModels
{
    public class CharacteristicDBModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string DataType { get; set; }

        public string? Unit { get; set; }

        public virtual ICollection<CategoryCharacteristicDBModel> CategoryCharacteristics { get; set; }
        public virtual ICollection<ProductCharacteristicDBModel> ProductCharacteristics { get; set; }
    }
}
