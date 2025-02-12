using System.Linq.Expressions;
using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Categories;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductServices
{
    public class CharacteristicService : ICharacteristicService
    {
        private readonly IRepository<CharacteristicDBModel> _repository;
        private readonly IMapper _mapper;

        public CharacteristicService(IRepository<CharacteristicDBModel> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<bool>> CreateAsync(CharacteristicCreateRequestModel request)
        {
            var model = _mapper.Map<CharacteristicDBModel>(request);
            var repoResult = await _repository.CreateAsync(model);
            return !repoResult.IsError
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(repoResult.Message, repoResult.Exception);
        }

        public async Task<OperationResultModel<bool>> UpdateAsync(CharacteristicRequestModel request)
        {
            var model = _mapper.Map<CharacteristicDBModel>(request);
            var repoResult = await _repository.UpdateAsync(model);
            return !repoResult.IsError
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(repoResult.Message, repoResult.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var repoResult = await _repository.DeleteAsync(id);
            return !repoResult.IsError
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(repoResult.Message, repoResult.Exception);
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
