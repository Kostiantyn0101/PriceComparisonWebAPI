using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class InstructionDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public int BaseProductId { get; set; }
        public string InstructionUrl { get; set; }

        public BaseProductDBModel BaseProduct { get; set; }
    }
}
