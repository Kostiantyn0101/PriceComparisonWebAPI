using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Primitives;
using Domain.Models.Request.Filters;
using Domain.Models.Response;
using Domain.Models.Response.Filters;
using System.Linq.Expressions;

namespace BLL.Services.FilterServices
{
    public class ProductFilterService : IProductFilterService
    {
        private readonly ICompositeKeyRepository<ProductFilterDBModel, int, int> _repository;
        private readonly IMapper _mapper;

        public ProductFilterService(ICompositeKeyRepository<ProductFilterDBModel, int, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<ProductFilterDBModel>> CreateAsync(ProductFilterCreateRequestModel request)
        {
            var model = _mapper.Map<ProductFilterDBModel>(request);
            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<ProductFilterDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<ProductFilterDBModel>> UpdateAsync(ProductFilterUpdateRequestModel request)
        {
            // Формуємо старий ключ
            var oldKey = new CompositeKey<int, int> { Key1 = request.OldProductId, Key2 = request.OldFilterId };
            var existingRecords = await _repository.GetFromConditionAsync(x =>
                x.ProductId == request.OldProductId && x.FilterId == request.OldFilterId);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<ProductFilterDBModel>.Failure("Product filter not found.");
            }

            // Видаляємо старий запис
            var deleteResult = await _repository.DeleteAsync(oldKey);
            if (!deleteResult.IsSuccess)
            {
                return OperationResultModel<ProductFilterDBModel>.Failure("Failed to delete the old product filter.", deleteResult.Exception);
            }

            // Створюємо новий запис із новими ключами
            var newModel = new ProductFilterDBModel
            {
                ProductId = request.NewProductId,
                FilterId = request.NewFilterId
            };

            var createResult = await _repository.CreateAsync(newModel);
            return createResult.IsSuccess
                ? createResult
                : OperationResultModel<ProductFilterDBModel>.Failure(createResult.ErrorMessage!, createResult.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int productId, int filterId)
        {
            var key = new CompositeKey<int, int> { Key1 = productId, Key2 = filterId };
            return await _repository.DeleteAsync(key);
        }

        public IQueryable<ProductFilterDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ProductFilterResponseModel>> GetFromConditionAsync(Expression<Func<ProductFilterDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<ProductFilterResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<ProductFilterDBModel>> ProcessQueryAsync(IQueryable<ProductFilterDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
