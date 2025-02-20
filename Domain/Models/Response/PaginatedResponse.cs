using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Response
{
    public class PaginatedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
    }
}
