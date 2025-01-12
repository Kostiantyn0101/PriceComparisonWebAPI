using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DBModels
{
    public class CategoryCharacteristicDBModel
    {
        [Required]
        public int CategoryId { get; set; }
        public virtual CategoryDBModel Category { get; set; }

        [Required]
        public int CharacteristicId { get; set; }
        public virtual CharacteristicDBModel Characteristic { get; set; }
    }
}
