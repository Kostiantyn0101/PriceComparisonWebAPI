using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.DBModels;
using Domain.Models.Request.Seller;
using Domain.Models.Response;
using Domain.Models.Response.Seller;

namespace BLL.Services.SellerServices
{
    public interface IAuctionClickRateService
    {
        Task<OperationResultModel<bool>> CreateAsync(AuctionClickRateCreateRequestModel request);

        Task<OperationResultModel<bool>> UpdateAsync(AuctionClickRateUpdateRequestModel request);

        Task<OperationResultModel<bool>> DeleteAsync(int id);

        IQueryable<AuctionClickRateDBModel> GetQuery();

        Task<IEnumerable<AuctionClickRateResponseModel>> GetFromConditionAsync(Expression<Func<AuctionClickRateDBModel, bool>> condition);

        Task<IEnumerable<AuctionClickRateDBModel>> ProcessQueryAsync(IQueryable<AuctionClickRateDBModel> query);
    }
}
