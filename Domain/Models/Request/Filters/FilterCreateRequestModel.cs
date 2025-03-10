namespace Domain.Models.Request.Filters
{
    public class FilterCreateRequestModel
    {
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Description { get; set; }
        public int CharacteristicId { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        public int CategoryId { get; set; }
        public bool DisplayOnProduct { get; set; }
    }
}
