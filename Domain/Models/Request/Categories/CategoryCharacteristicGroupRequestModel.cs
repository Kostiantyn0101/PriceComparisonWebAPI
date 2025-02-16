
namespace Domain.Models.Request.Categories
{
    public class CategoryCharacteristicGroupRequestModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int CharacteristicGroupId { get; set; }
        public int GroupDisplayOrder { get; set; }
    }
}
