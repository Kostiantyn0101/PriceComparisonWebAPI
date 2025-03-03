using System.Linq.Expressions;
using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductServices
{
    public class InstructionService : IInstructionService
    {
        private readonly IRepository<InstructionDBModel, int> _repository;
        private readonly IMapper _mapper;

        public InstructionService(IRepository<InstructionDBModel, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<bool>> CreateAsync(InstructionCreateRequestModel request)
        {
            var model = _mapper.Map<InstructionDBModel>(request);
            var result = await _repository.CreateAsync(model);
            return result.IsSuccess
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(result.ErrorMessage!, result.Exception);
        }

        public async Task<OperationResultModel<bool>> UpdateAsync(InstructionUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<bool>.Failure("Instruction not found.");
            }

            _mapper.Map(request, existing);

            var result = await _repository.UpdateAsync(existing);
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
