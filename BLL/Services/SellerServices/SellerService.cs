using System.Linq.Expressions;
using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Seller;
using Domain.Models.Response;
using Domain.Models.Response.Seller;

namespace BLL.Services.SellerServices
{
    public class SellerService : ISellerService
    {
        private readonly IRepository<SellerDBModel> _repository;
        private readonly IMapper _mapper;

        public SellerService(IRepository<SellerDBModel> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<bool>> CreateAsync(SellerCreateRequestModel request)
        {
            var model = _mapper.Map<SellerDBModel>(request);
            model.ApiKey = Guid.NewGuid().ToString();
            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<bool>> UpdateAsync(SellerUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<bool>.Failure("Seller not found.");
            }

            _mapper.Map(request, existing);

            var repoResult = await _repository.UpdateAsync(existing);
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

        public IQueryable<SellerDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<SellerResponseModel>> GetFromConditionAsync(Expression<Func<SellerDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<SellerResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<SellerDBModel>> ProcessQueryAsync(IQueryable<SellerDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
