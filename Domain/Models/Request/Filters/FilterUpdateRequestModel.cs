namespace Domain.Models.Request.Filters
{
    public class FilterUpdateRequestModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Description { get; set; }
    }
}
