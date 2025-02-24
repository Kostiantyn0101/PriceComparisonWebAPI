using Domain.Models.Response.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ProductServices
{
    public interface IProductComparisonService
    {
        Task<(IEnumerable<ProductCharacteristicGroupResponseModel> ProductA,
              IEnumerable<ProductCharacteristicGroupResponseModel> ProductB,
              string Explanation)> CompareProductsAsync(int productIdA, int productIdB);
    }
}
