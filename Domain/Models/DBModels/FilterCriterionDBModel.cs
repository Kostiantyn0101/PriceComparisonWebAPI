using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class FilterCriterionDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public int FilterId { get; set; }
        public FilterDBModel Filter { get; set; }

        public int CharacteristicId { get; set; }
        public CharacteristicDBModel Characteristic { get; set; }

        public string Operator { get; set; }
        public decimal Value { get; set; }
    }
}
