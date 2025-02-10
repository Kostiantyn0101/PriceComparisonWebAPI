namespace Domain.Models.Request.Products
{
    public class ProductCharacteristicValueUpdateModel
    {
        public int CharacteristicId { get; set; }
        public string? ValueText { get; set; }
        public decimal? ValueNumber { get; set; }
        public bool? ValueBoolean { get; set; }
        public DateTime? ValueDate { get; set; }
    }
}
