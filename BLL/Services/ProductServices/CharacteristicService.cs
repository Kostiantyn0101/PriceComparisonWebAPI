using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using System.Linq.Expressions;

namespace BLL.Services.ProductServices
{
    public class CharacteristicService : ICharacteristicService
    {
        private readonly IRepository<CharacteristicDBModel, int> _repository;
        private readonly IMapper _mapper;

        public CharacteristicService(IRepository<CharacteristicDBModel, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<CharacteristicResponseModel>> CreateAsync(CharacteristicCreateRequestModel request)
        {
            var model = _mapper.Map<CharacteristicDBModel>(request);
            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? OperationResultModel<CharacteristicResponseModel>.Success(_mapper.Map<CharacteristicResponseModel>(repoResult.Data))
                : OperationResultModel<CharacteristicResponseModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<CharacteristicDBModel>> UpdateAsync(CharacteristicRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<CharacteristicDBModel>.Failure("Characteristic not found.");
            }

            _mapper.Map(request, existing);

            var result = await _repository.UpdateAsync(existing);
            return result.IsSuccess
                ? result
                : OperationResultModel<CharacteristicDBModel>.Failure(result.ErrorMessage!, result.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var repoResult = await _repository.DeleteAsync(id);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public IQueryable<CharacteristicDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<CharacteristicResponseModel>> GetFromConditionAsync(Expression<Func<CharacteristicDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<CharacteristicResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<CharacteristicDBModel>> ProcessQueryAsync(IQueryable<CharacteristicDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
