using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using System.Linq.Expressions;

namespace BLL.Services.ProductServices
{
    public class CharacteristicGroupService : ICharacteristicGroupService
    {
        private readonly IRepository<CharacteristicGroupDBModel, int> _repository;
        private readonly IMapper _mapper;

        public CharacteristicGroupService(IRepository<CharacteristicGroupDBModel, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<CharacteristicGroupDBModel>> CreateAsync(CharacteristicGroupCreateRequestModel request)
        {
            var model = _mapper.Map<CharacteristicGroupDBModel>(request);
            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<CharacteristicGroupDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<CharacteristicGroupDBModel>> UpdateAsync(CharacteristicGroupRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<CharacteristicGroupDBModel>.Failure("Characteristic not found.");
            }

            _mapper.Map(request, existing);

            var result = await _repository.UpdateAsync(existing);
            return result.IsSuccess
                ? result
                : OperationResultModel<CharacteristicGroupDBModel>.Failure(result.ErrorMessage!, result.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var repoResult = await _repository.DeleteAsync(id);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public IQueryable<CharacteristicGroupDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<CharacteristicGroupResponseModel>> GetFromConditionAsync(Expression<Func<CharacteristicGroupDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<CharacteristicGroupResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<CharacteristicGroupDBModel>> ProcessQueryAsync(IQueryable<CharacteristicGroupDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
