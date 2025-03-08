using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Primitives;
using Domain.Models.Request.Filters;
using Domain.Models.Response;
using Domain.Models.Response.Filters;
using Domain.Models.Response.Products;
using System.Linq.Expressions;

namespace BLL.Services.FilterServices
{
    public class ProductFilterService : IProductFilterService
    {
        private readonly ICompositeKeyRepository<ProductFilterDBModel, int, int> _repository;
        private readonly IRepository<FilterDBModel, int> _filterRepository;
        private readonly IRepository<ProductDBModel, int> _productRepository;
        private readonly IMapper _mapper;

        public ProductFilterService(
            ICompositeKeyRepository<ProductFilterDBModel, int, int> repository,
            IRepository<FilterDBModel, int> filterRepository,
            IRepository<ProductDBModel, int> productRepository,
            IMapper mapper)
        {
            _repository = repository;
            _filterRepository = filterRepository;
            _productRepository = productRepository;
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
            var oldKey = new CompositeKey<int, int> { Key1 = request.OldProductId, Key2 = request.OldFilterId };
            var existingRecords = await _repository.GetFromConditionAsync(x =>
                x.ProductId == request.OldProductId && x.FilterId == request.OldFilterId);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<ProductFilterDBModel>.Failure("Product filter not found.");
            }

            var deleteResult = await _repository.DeleteAsync(oldKey);
            if (!deleteResult.IsSuccess)
            {
                return OperationResultModel<ProductFilterDBModel>.Failure("Failed to delete the old product filter.", deleteResult.Exception);
            }

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

        public async Task<IEnumerable<FilterResponseModel>> GetFiltersByProductIdAsync(int productId)
        {
            var pfRecords = await GetFromConditionAsync(x => x.ProductId == productId);
            var filterIds = pfRecords.Select(x => x.FilterId).Distinct().ToList();
            var filterRecords = await _filterRepository.GetFromConditionAsync(x => filterIds.Contains(x.Id));
            return _mapper.Map<IEnumerable<FilterResponseModel>>(filterRecords);
        }

        public async Task<IEnumerable<ProductResponseModel>> GetProductsByFilterIdAsync(int filterId)
        {
            var pfRecords = await GetFromConditionAsync(x => x.FilterId == filterId);
            var productIds = pfRecords.Select(x => x.ProductId).Distinct().ToList();
            var productRecords = await _productRepository.GetFromConditionAsync(x => productIds.Contains(x.Id));
            return _mapper.Map<IEnumerable<ProductResponseModel>>(productRecords);
        }
    }
}
