using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Request.Categories
{
    public class RelatedCategoryUpdateRequestModel
    {
        public int OldCategoryId { get; set; }
        public int OldRelatedCategoryId { get; set; }

        public int NewCategoryId { get; set; }
        public int NewRelatedCategoryId { get; set; }
    }
}
