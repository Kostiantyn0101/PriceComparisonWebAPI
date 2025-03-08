namespace Domain.Models.Request.Filters
{
    public class FilterCriterionCreateRequestModel
    {
        public int FilterId { get; set; }
        public int CharacteristicId { get; set; }
        public string Operator { get; set; }
        public decimal Value { get; set; }
    }
}
