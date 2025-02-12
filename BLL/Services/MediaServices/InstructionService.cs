using System.Linq.Expressions;
using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.MediaServices
{
    public class InstructionService : IInstructionService
    {
        private readonly IRepository<InstructionDBModel> _repository;
        private readonly IMapper _mapper;

        public InstructionService(IRepository<InstructionDBModel> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<bool>> CreateAsync(InstructionCreateRequestModel request)
        {
            var model = _mapper.Map<InstructionDBModel>(request);
            var result = await _repository.CreateAsync(model);
            return !result.IsError
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(result.Message, result.Exception);
        }

        public async Task<OperationResultModel<bool>> UpdateAsync(InstructionUpdateRequestModel request)
        {
            var model = _mapper.Map<InstructionDBModel>(request);
            var result = await _repository.UpdateAsync(model);
            return !result.IsError
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(result.Message, result.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var result = await _repository.DeleteAsync(id);
            return !result.IsError
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(result.Message, result.Exception);
        }

        public IQueryable<InstructionDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<InstructionResponseModel>> GetFromConditionAsync(Expression<Func<InstructionDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<InstructionResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<InstructionDBModel>> ProcessQueryAsync(IQueryable<InstructionDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
