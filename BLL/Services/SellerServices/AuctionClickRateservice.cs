using System.Linq.Expressions;
using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Seller;
using Domain.Models.Response;
using Domain.Models.Response.Seller;

namespace BLL.Services.SellerServices
{
    public class AuctionClickRateService : IAuctionClickRateService
    {
        private readonly IRepository<AuctionClickRateDBModel, int> _repository;
        private readonly IMapper _mapper;

        public AuctionClickRateService(IRepository<AuctionClickRateDBModel, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<AuctionClickRateDBModel>> CreateAsync(AuctionClickRateCreateRequestModel request)
        {
            var model = _mapper.Map<AuctionClickRateDBModel>(request);
            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<AuctionClickRateDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<AuctionClickRateDBModel>> UpdateAsync(AuctionClickRateUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<AuctionClickRateDBModel>.Failure("AuctionClickRate record not found.");
            }

            _mapper.Map(request, existing);

            var repoResult = await _repository.UpdateAsync(existing);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<AuctionClickRateDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var repoResult = await _repository.DeleteAsync(id);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public IQueryable<AuctionClickRateDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<AuctionClickRateResponseModel>> GetFromConditionAsync(Expression<Func<AuctionClickRateDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<AuctionClickRateResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<AuctionClickRateDBModel>> ProcessQueryAsync(IQueryable<AuctionClickRateDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
