using System.Linq.Expressions;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.MediaServices
{
    public class InstructionService : IInstructionService
    {
        private readonly IRepository<InstructionDBModel> _repository;

        public InstructionService(IRepository<InstructionDBModel> repository)
        {
            _repository = repository;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(InstructionDBModel model)
        {
            return await _repository.CreateAsync(model);
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(InstructionDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public IQueryable<InstructionDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<InstructionDBModel>> GetFromConditionAsync(Expression<Func<InstructionDBModel, bool>> condition)
        {
            return await _repository.GetFromConditionAsync(condition);
        }

        public async Task<IEnumerable<InstructionDBModel>> ProcessQueryAsync(IQueryable<InstructionDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
