using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.MediaServices
{
    public interface IInstructionService
    {
        Task<OperationResultModel<bool>> CreateAsync(InstructionCreateRequestModel model);
        Task<OperationResultModel<bool>> UpdateAsync(InstructionUpdateRequestModel entity);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<InstructionDBModel> GetQuery();
        Task<IEnumerable<InstructionResponseModel>> GetFromConditionAsync(Expression<Func<InstructionDBModel, bool>> condition);
        Task<IEnumerable<InstructionDBModel>> ProcessQueryAsync(IQueryable<InstructionDBModel> query);
    }
}
