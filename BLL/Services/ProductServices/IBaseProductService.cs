using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.DBModels;
using Domain.Models.Response.Products;
using Domain.Models.Response;
using Domain.Models.Request.Products;

namespace BLL.Services.ProductServices
{
    public interface IBaseProductService
    {
        Task<OperationResultModel<BaseProductDBModel>> CreateAsync(BaseProductCreateRequestModel request);
        Task<OperationResultModel<BaseProductDBModel>> UpdateAsync(BaseProductUpdateRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<BaseProductDBModel> GetQuery();
        Task<IEnumerable<BaseProductResponseModel>> GetFromConditionAsync(Expression<Func<BaseProductDBModel, bool>> condition);
        Task<IEnumerable<BaseProductDBModel>> ProcessQueryAsync(IQueryable<BaseProductDBModel> query);
    }
}
