using System.ComponentModel.DataAnnotations;


namespace Domain.Models.DBModels
{
    public class ReviewDBModel
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public virtual ProductDBModel Product { get; set; }

        [Required]
        public string ReviewUrl { get; set; }
    }
}
