using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Domain.Models.DTO.Categories
{
    public class CategoryCreateRequestModel
    {
        public string Title { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFile? Icon { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
