using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Entities
{
    public class Instruction
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        [MaxLength(2083)]
        public string InstructionUrl { get; set; }
    }
}
