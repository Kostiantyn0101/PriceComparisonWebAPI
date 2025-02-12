namespace Domain.Models.Request.Products
{
    public class InstructionUpdateRequestModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string InstructionUrl { get; set; }
    }
}
