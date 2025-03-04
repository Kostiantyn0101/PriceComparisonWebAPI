using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Primitives;
using Domain.Models.Request.Categories;
using Domain.Models.Response;
using Domain.Models.Response.Categories;
using System.Linq.Expressions;

namespace BLL.Services.CategoryService
{
    public class RelatedCategoryService : IRelatedCategoryService
    {
        private readonly ICompositeKeyRepository<RelatedCategoryDBModel, int, int> _repository;

        private readonly IMapper _mapper;

        public RelatedCategoryService(ICompositeKeyRepository<RelatedCategoryDBModel, int, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<RelatedCategoryDBModel>> CreateAsync(RelatedCategoryRequestModel request)
        {
            var model = _mapper.Map<RelatedCategoryDBModel>(request);
            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<RelatedCategoryDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<RelatedCategoryDBModel>> UpdateAsync(RelatedCategoryUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x =>
                x.CategoryId == request.OldCategoryId && x.RelatedCategoryId == request.OldRelatedCategoryId);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<RelatedCategoryDBModel>.Failure("Entity not found.");
            }

            var deleteResult = await DeleteAsync(request.OldCategoryId, request.OldRelatedCategoryId);
            if (!deleteResult.IsSuccess)
            {
                return OperationResultModel<RelatedCategoryDBModel>.Failure(deleteResult.ErrorMessage!, deleteResult.Exception);
            }
            var newEntity = new RelatedCategoryDBModel
            {
                CategoryId = request.NewCategoryId,
                RelatedCategoryId = request.NewRelatedCategoryId,
            };

            var createResult = await _repository.CreateAsync(newEntity);
            return createResult.IsSuccess
                ? createResult
                : OperationResultModel<RelatedCategoryDBModel>.Failure(createResult.ErrorMessage!, createResult.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int categoryId, int relatedCategoryId)
        {
            var compositeKey = new CompositeKey<int, int> { Key1 = categoryId, Key2 = relatedCategoryId };
            var repoResult = await _repository.DeleteAsync(compositeKey);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public IQueryable<RelatedCategoryDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<RelatedCategoryResponseModel>> GetFromConditionAsync(Expression<Func<RelatedCategoryDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<RelatedCategoryResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<RelatedCategoryDBModel>> ProcessQueryAsync(IQueryable<RelatedCategoryDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
