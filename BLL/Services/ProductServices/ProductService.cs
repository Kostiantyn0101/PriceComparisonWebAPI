using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BLL.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IRepository<ProductDBModel, int> _repository;
        private readonly IRepository<BaseProductDBModel, int> _baseProductRepository;
        private readonly IMapper _mapper;

        public ProductService(IRepository<ProductDBModel, int> repository,
            IRepository<BaseProductDBModel, int> baseProductRepository,
            IMapper mapper)
        {
            _repository = repository;
            _baseProductRepository = baseProductRepository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<ProductDBModel>> CreateAsync(ProductCreateRequestModel model)
        {
            var mapped = _mapper.Map<ProductDBModel>(model);
            mapped.AddedToDatabase = DateTime.UtcNow;
            var result = await _repository.CreateAsync(mapped);
            return result.IsSuccess
                ? result
                : OperationResultModel<ProductDBModel>.Failure(result.ErrorMessage!, result.Exception);
        }

        public async Task<OperationResultModel<ProductDBModel>> UpdateAsync(ProductUpdateRequestModel model)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == model.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<ProductDBModel>.Failure("Product not found.");
            }

            _mapper.Map(model, existing);

            var result = await _repository.UpdateAsync(existing);
            return result.IsSuccess
                ? result
                : OperationResultModel<ProductDBModel>.Failure(result.ErrorMessage!, result.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var result = await _repository.DeleteAsync(id);
            return result.IsSuccess
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(result.ErrorMessage!, result.Exception);
        }

        public async Task<OperationResultModel<PaginatedResponse<ProductResponseModel>>> GetPaginatedProductsAsync(
                    Expression<Func<ProductDBModel, bool>> condition, int page, int pageSize)
        {
            var query = _repository.GetQuery().Where(condition);

            var totalItems = await query.CountAsync();

            var products = await query.Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

            var mappedProducts = _mapper.Map<IEnumerable<ProductResponseModel>>(products);

            var response = new PaginatedResponse<ProductResponseModel>
            {
                Data = mappedProducts,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return OperationResultModel<PaginatedResponse<ProductResponseModel>>.Success(response);
        }

        public async Task<OperationResultModel<PaginatedResponse<BaseProductByCategoryResponseModel>>> GetPaginatedProductsByCategoryAsync(
              int categoryId, int page, int pageSize)
        {
            var query = _baseProductRepository.GetQuery()
                .Where(bp => bp.CategoryId == categoryId && !bp.IsUnderModeration)
                .Select(bp => new BaseProductByCategoryResponseModel()
                {
                    Title = bp.Title,
                    Description = bp.Description,
                    BaseProductId = bp.Id,
                    CategoryId = categoryId,
                    Brand = bp.Brand,
                    //Products = bp.Products
                    //.Where(p => !p.IsUnderModeration)
                    //.Select(p => new ProductResponseModel()
                    //{
                    //    ProductId = p.Id,
                    //    ModelNumber = p.ModelNumber,
                    //    ProductGroupId = p.ProductGroupId,
                    //})
                    //.ToList(),
                    ProductGroups = bp.Products
                    .Where(p => !p.IsUnderModeration)
                    .GroupBy(p => p.ProductGroupId)
                    .Select(p => new ProductGroupModifiedResponseModel()
                    {
                        Id = p.Key,
                        Name = p.First().ProductGroup.Name,
                        FirstProductId = p.First().Id,
                        DisplayOrder = 1
                    })
                    .OrderBy(pg => pg.DisplayOrder)
                    .ToList()
                });

            var totalItems = await query.CountAsync();

            var products = await query.Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();
            
            var response = new PaginatedResponse<BaseProductByCategoryResponseModel>
            {
                Data = products,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return OperationResultModel<PaginatedResponse<BaseProductByCategoryResponseModel>>.Success(response);
        }

        public IQueryable<ProductDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ProductResponseModel>> GetFromConditionAsync(Expression<Func<ProductDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<ProductResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<ProductDBModel>> ProcessQueryAsync(IQueryable<ProductDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
