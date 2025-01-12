using System.ComponentModel.DataAnnotations;


namespace Domain.Models.Entities
{
    public class ProductCharacteristic
    {
        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public int CharacteristicId { get; set; }
        public virtual Characteristic Characteristic { get; set; }

        public string ValueText { get; set; }
        public decimal? ValueNumber { get; set; }
        public bool? ValueBoolean { get; set; }
        public DateTime? ValueDate { get; set; }
    }

}
