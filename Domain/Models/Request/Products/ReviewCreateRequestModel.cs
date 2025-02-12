using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Request.Products
{
    public class ReviewCreateRequestModel
    {
        public int ProductId { get; set; }
        public string ReviewUrl { get; set; }
    }
}
