using System.Linq.Expressions;
using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.ProductServices
{
    public class ColorService : IColorService
    {
        private readonly IRepository<ColorDBModel, int> _repository;
        private readonly IRepository<ProductDBModel, int> _productRepository;
        private readonly IMapper _mapper;

        public ColorService(IRepository<ColorDBModel, int> repository, IRepository<ProductDBModel, int> productRepository, IMapper mapper)
        {
            _repository = repository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<ColorDBModel>> CreateAsync(ColorCreateRequestModel request)
        {
            var model = _mapper.Map<ColorDBModel>(request);
            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<ColorDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<ColorDBModel>> UpdateAsync(ColorUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<ColorDBModel>.Failure("Color record not found.");
            }

            _mapper.Map(request, existing);

            var repoResult = await _repository.UpdateAsync(existing);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<ColorDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var repoResult = await _repository.DeleteAsync(id);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public IQueryable<ColorDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ColorResponseModel>> GetFromConditionAsync(Expression<Func<ColorDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<ColorResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<ColorDBModel>> ProcessQueryAsync(IQueryable<ColorDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }

        public async Task<IEnumerable<ProductColorResponseModel>> GetByProductGroupIdAsync(int productGroupId)
        {
            var query = _productRepository.GetQuery()
                .Where(p => p.ProductGroupId == productGroupId && p.ColorId != null)
                .Select(p => new ProductColorResponseModel()
                {
                    ProductId = p.Id,
                    Name = p.Color!.Name,
                    HexCode = p.Color!.HexCode,
                    GradientCode = p.Color!.GradientCode,
                });

            return await query.ToListAsync();
        }
    }
}
