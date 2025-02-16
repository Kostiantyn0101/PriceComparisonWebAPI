
namespace Domain.Models.Response.Categories
{
    public class CategoryCharacteristicGroupResponseModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int CharacteristicGroupId { get; set; }
        public int GroupDisplayOrder { get; set; }
    }
}
