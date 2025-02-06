namespace Domain.Models.Request.Categories
{
    public class CategoryCharacteristicRequestModel
    {
        public int CategoryId { get; set; }
        public List<int> CharacteristicIds { get; set; }
    }
}
