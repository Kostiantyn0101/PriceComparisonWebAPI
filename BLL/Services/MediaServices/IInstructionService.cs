using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.MediaServices
{
    public interface IInstructionService
    {
        Task<OperationDetailsResponseModel> CreateAsync(InstructionDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(InstructionDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int id);
        IQueryable<InstructionDBModel> GetQuery();
        Task<IEnumerable<InstructionDBModel>> GetFromConditionAsync(Expression<Func<InstructionDBModel, bool>> condition);
        Task<IEnumerable<InstructionDBModel>> ProcessQueryAsync(IQueryable<InstructionDBModel> query);
    }
}
