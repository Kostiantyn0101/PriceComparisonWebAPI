using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Response.Products;

namespace Domain.Models.Extensions
{
    public static class PopularProductExtensions
    {
        public static void ApplyServerUrl(this PopularProductResponseModel product, string serverBaseUrl)
        {
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                product.ImageUrl = $"{serverBaseUrl.TrimEnd('/')}/{product.ImageUrl.TrimStart('/')}";
            }
        }
    }
}
