using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Response.Products
{
    public class ReviewResponseModel
    {
        public int Id { get; set; }
        public int BaseProductId { get; set; }
        public string ReviewUrl { get; set; }
    }
}
