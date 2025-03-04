using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using System.Linq.Expressions;

namespace BLL.Services.ProductServices
{
    public interface IInstructionService
    {
        Task<OperationResultModel<InstructionDBModel>> CreateAsync(InstructionCreateRequestModel model);
        Task<OperationResultModel<InstructionDBModel>> UpdateAsync(InstructionUpdateRequestModel entity);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<InstructionDBModel> GetQuery();
        Task<IEnumerable<InstructionResponseModel>> GetFromConditionAsync(Expression<Func<InstructionDBModel, bool>> condition);
        Task<IEnumerable<InstructionDBModel>> ProcessQueryAsync(IQueryable<InstructionDBModel> query);
    }
}
