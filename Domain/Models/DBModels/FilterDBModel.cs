using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class FilterDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Description { get; set; }
        public int CharacteristicId { get; set; }
        public CharacteristicDBModel Characteristic { get; set; }
        public string Operator { get; set; } // "==", ">=", "<=", ">", "<", "!="
        public string Value { get; set; }
        public int CategoryId { get; set; }
        public bool DisplayOnProduct { get; set; }
    }
}
