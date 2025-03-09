using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class BaseProductDBModel : IEntity<int>
    {
        private string _title;
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                NormalizedTitle = value?.Trim().ToUpperInvariant(); ;
            }
        }
        public string NormalizedTitle { get; private set; }
        public string? Description { get; set; }
        public bool IsUnderModeration { get; set; }    //+
        public DateTime AddedToDatabase { get; set; }  //+

        public int CategoryId { get; set; }
        public CategoryDBModel Category { get; set; }

        public ICollection<ProductVideoDBModel> ProductVideos { get; set; }
        public ICollection<FeedbackDBModel> Feedbacks { get; set; }
        public ICollection<ReviewDBModel> Reviews { get; set; }
        public ICollection<InstructionDBModel> Instructions { get; set; }
        public ICollection<ProductDBModel> Products { get; set; }
    }
}
