using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class InstructionDBModel : EntityDBModel
    {
        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public string InstructionUrl { get; set; }
    }
}
