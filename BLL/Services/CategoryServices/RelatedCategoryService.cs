using System.Linq.Expressions;
using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Categories;
using Domain.Models.Response;
using Domain.Models.Response.Categories;
using Domain.Models.Response.Products;

namespace BLL.Services.CategoryService
{
    public class RelatedCategoryService : IRelatedCategoryService
    {
        private readonly IRelatedCategoryRepository _repository;
        private readonly IMapper _mapper;

        public RelatedCategoryService(IRelatedCategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<bool>> CreateAsync(RelatedCategoryRequestModel request)
        {
            var model = _mapper.Map<RelatedCategoryDBModel>(request);
            var repoResult = await _repository.CreateAsync(model);
            return !repoResult.IsError
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(repoResult.Message, repoResult.Exception);
        }

        public async Task<OperationResultModel<bool>> UpdateAsync(RelatedCategoryUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x =>
                x.CategoryId == request.OldCategoryId && x.RelatedCategoryId == request.OldRelatedCategoryId);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<bool>.Failure("Entity not found.");
            }

            var deleteResult = await _repository.DeleteAsync(request.OldCategoryId, request.OldRelatedCategoryId);
            if (deleteResult.IsError)
            {
                return OperationResultModel<bool>.Failure(deleteResult.Message, deleteResult.Exception);
            }
            var newEntity = new RelatedCategoryDBModel
            {
                CategoryId = request.NewCategoryId,
                RelatedCategoryId = request.NewRelatedCategoryId,
            };

            var createResult = await _repository.CreateAsync(newEntity);
            return !createResult.IsError
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(createResult.Message, createResult.Exception);

        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int categoryId, int relatedCategoryId)
        {
            var repoResult = await _repository.DeleteAsync(categoryId, relatedCategoryId);
            return !repoResult.IsError
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(repoResult.Message, repoResult.Exception);
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
