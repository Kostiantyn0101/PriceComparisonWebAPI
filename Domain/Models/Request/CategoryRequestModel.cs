using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Request
{
    public class CategoryRequestModel
    {
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? IconUrl { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
