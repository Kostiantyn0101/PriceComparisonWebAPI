using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Categories;
using Domain.Models.Response;
using Domain.Models.Response.Categories;
using System.Linq.Expressions;

namespace BLL.Services.CategoryServices
{
    public class CategoryCharacteristicGroupService : ICategoryCharacteristicGroupService
    {
        private readonly IRepository<CategoryCharacteristicGroupDBModel, int> _repository;
        private readonly IMapper _mapper;

        public CategoryCharacteristicGroupService(IRepository<CategoryCharacteristicGroupDBModel, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<CategoryCharacteristicGroupDBModel>> CreateAsync(CategoryCharacteristicGroupCreateRequestModel request)
        {
            var model = _mapper.Map<CategoryCharacteristicGroupDBModel>(request);
            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<CategoryCharacteristicGroupDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<CategoryCharacteristicGroupDBModel>> UpdateAsync(CategoryCharacteristicGroupRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<CategoryCharacteristicGroupDBModel>.Failure("CategoryCharacteristicGroup not found.");
            }

            _mapper.Map(request, existing);

            var result = await _repository.UpdateAsync(existing);
            return result.IsSuccess
                ? result
                : OperationResultModel<CategoryCharacteristicGroupDBModel>.Failure(result.ErrorMessage!, result.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var repoResult = await _repository.DeleteAsync(id);
            return repoResult.IsSuccess
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public IQueryable<CategoryCharacteristicGroupDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<CategoryCharacteristicGroupResponseModel>> GetFromConditionAsync(Expression<Func<CategoryCharacteristicGroupDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<CategoryCharacteristicGroupResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<CategoryCharacteristicGroupDBModel>> ProcessQueryAsync(IQueryable<CategoryCharacteristicGroupDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
