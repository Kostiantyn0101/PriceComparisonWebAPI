using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DBModels
{
    public class ProductCharacteristicDBModel
    {
        [Required]
        public int ProductId { get; set; }
        public virtual ProductDBModel Product { get; set; }

        [Required]
        public int CharacteristicId { get; set; }
        public virtual CharacteristicDBModel Characteristic { get; set; }

        public string ValueText { get; set; }
        public decimal? ValueNumber { get; set; }
        public bool? ValueBoolean { get; set; }
        public DateTime? ValueDate { get; set; }
    }

}
