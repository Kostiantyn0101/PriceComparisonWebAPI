namespace Domain.Models.Response.Filters
{
    public class FilterCriterionResponseModel
    {
        public int Id { get; set; }
        public int FilterId { get; set; }
        public int CharacteristicId { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }
}
