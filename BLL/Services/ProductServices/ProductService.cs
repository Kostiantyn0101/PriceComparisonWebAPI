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
        private readonly IMapper _mapper;

        public ProductService(IRepository<ProductDBModel, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<ProductDBModel>> CreateAsync(ProductRequestModel model)
        {
            var mapped = _mapper.Map<ProductDBModel>(model);
            var result = await _repository.CreateAsync(mapped);
            return result.IsSuccess
                ? result
                : OperationResultModel<ProductDBModel>.Failure(result.ErrorMessage!, result.Exception);
        }

        public async Task<OperationResultModel<ProductDBModel>> UpdateAsync(ProductRequestModel model)
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
