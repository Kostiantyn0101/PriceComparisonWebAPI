using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DBModels
{
    public class RoleDBModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual ICollection<UserDBModel> Users { get; set; }
    }
}
