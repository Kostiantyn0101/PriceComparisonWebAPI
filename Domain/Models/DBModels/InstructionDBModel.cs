namespace Domain.Models.DBModels
{
    public class InstructionDBModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public string InstructionUrl { get; set; }
    }
}
