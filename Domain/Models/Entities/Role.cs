using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Entities
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
