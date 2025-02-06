namespace Domain.Models.Response.Categories
{
    public class CategoryCharacteristicResponseModel
    {
        public int CategoryId { get; set; }
        public int CharacteristicId { get; set; }
        public string CharacteristicTitle { get; set; }
        public string CharacteristicDataType { get; set; }
        public string? CharacteristicUnit { get; set; }

    }
}