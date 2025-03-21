namespace Domain.Models.Request.Products
{
    public class InstructionUpdateRequestModel
    {
        public int Id { get; set; }
        public int BaseProductId { get; set; }
        public string InstructionUrl { get; set; }
    }
}
