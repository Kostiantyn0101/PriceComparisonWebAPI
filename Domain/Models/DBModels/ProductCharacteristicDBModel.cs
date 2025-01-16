namespace Domain.Models.DBModels
{
    public class ProductCharacteristicDBModel
    {
        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public int CharacteristicId { get; set; }
        public CharacteristicDBModel Characteristic { get; set; }

        public string? ValueText { get; set; }
        public decimal? ValueNumber { get; set; }
        public bool? ValueBoolean { get; set; }
        public DateTime? ValueDate { get; set; }
    }

}
