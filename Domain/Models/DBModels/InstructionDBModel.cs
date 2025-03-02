using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class InstructionDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public string InstructionUrl { get; set; }
    }
}
