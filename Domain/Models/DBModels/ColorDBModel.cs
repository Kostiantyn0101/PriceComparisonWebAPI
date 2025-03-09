using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ColorDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HexCode { get; set; }
        public string? GradientCode { get; set; }

        //public ICollection<FeedbackImageDBModel> FeedbackImages { get; set; }
    }
}
