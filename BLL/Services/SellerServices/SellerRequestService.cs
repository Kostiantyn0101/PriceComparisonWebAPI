using System.Linq.Expressions;
using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Seller;
using Domain.Models.Response.Seller;
using Domain.Models.Response;
using BLL.Services.Auth;
using Domain.Models.Request.Auth;
using Microsoft.AspNetCore.Identity;
using Domain.Models.Identity;

namespace BLL.Services.SellerServices
{
    public class SellerRequestService : ISellerRequestService
    {
        private readonly IRepository<SellerRequestDBModel, int> _repository;
        private readonly IRepository<SellerDBModel, int> _sellerRepository;
        private readonly ISellerService _sellerService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public SellerRequestService(IRepository<SellerRequestDBModel, int> repository, 
            IRepository<SellerDBModel, int> sellerRepository,
            ISellerService sellerService,
            IAuthService authService,
            IMapper mapper)
        {
            _repository = repository;
            _sellerRepository = sellerRepository;
            _sellerService = sellerService;
            _authService = authService;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<SellerRequestDBModel>> CreateAsync(SellerRequestCreateRequestModel request)
        {
            var model = _mapper.Map<SellerRequestDBModel>(request);
            model.CreatedAt = DateTime.UtcNow;
            model.IsProcessed = false;
            model.IsApproved = false;

            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<SellerRequestDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<SellerRequestDBModel>> UpdateAsync(SellerRequestUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<SellerRequestDBModel>.Failure("SellerRequest record not found.");
            }

            _mapper.Map(request, existing);
            var repoResult = await _repository.UpdateAsync(existing);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<SellerRequestDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var repoResult = await _repository.DeleteAsync(id);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<SellerRequestDBModel>> ProcessRequestAsync(SellerRequestProcessRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existingRequest = existingRecords.FirstOrDefault();
            if (existingRequest == null)
            {
                return OperationResultModel<SellerRequestDBModel>.Failure("SellerRequest record not found.");
            }

            existingRequest.IsProcessed = true;
            existingRequest.IsApproved = request.IsApproved;
            existingRequest.ProcessedAt = DateTime.UtcNow;

            var existingSellerRecords = await _sellerRepository.GetFromConditionAsync(x => x.UserId == existingRequest.UserId);
            var existingSeller = existingSellerRecords.FirstOrDefault();

            if (request.IsApproved)
            {
                if (existingSeller == null)
                {
                    var newSeller = new SellerCreateRequestModel
                    {
                        AccountBalance = 0,
                        UserId = existingRequest.UserId,
                        IsActive = true,
                        PublishPriceList = true,
                        StoreName = existingRequest.StoreName,
                        WebsiteUrl = existingRequest.WebsiteUrl,
                    };

                    var sellerCreateResult = await _sellerService.CreateAsync(newSeller);
                    if (!sellerCreateResult.IsSuccess)
                    {
                        return OperationResultModel<SellerRequestDBModel>.Failure("Failed to create a new seller");
                    }

                    var roleChangeResult = await _authService.UpdateUserRolesAsync(new UpdateUserRolesRequestModel
                    {
                        UserId = existingRequest.UserId,
                        Roles = new List<string> { Role.Seller }
                    });
                    if (!roleChangeResult.IsSuccess)
                    {
                        return OperationResultModel<SellerRequestDBModel>.Failure("New seller created. Failed to change user role");
                    }
                }
                else
                {
                    existingSeller.IsActive = true;

                    var sellerUpdateResult = await _sellerRepository.UpdateAsync(existingSeller);
                    if (!sellerUpdateResult.IsSuccess)
                    {
                        return OperationResultModel<SellerRequestDBModel>.Failure("Fail to activate seller");
                    }
                }
            }
            else
            {
                existingRequest.RefusalReason = request.RefusalReason;

                if (existingSeller != null)
                {
                    existingSeller.IsActive = false;

                    var sellerUpdateResult = await _sellerRepository.UpdateAsync(existingSeller);
                    if (!sellerUpdateResult.IsSuccess)
                    {
                        return OperationResultModel<SellerRequestDBModel>.Failure("Fail to deactivate seller");
                    }
                }
            }

            var repoResult = await _repository.UpdateAsync(existingRequest);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<SellerRequestDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public IQueryable<SellerRequestDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<SellerRequestResponseModel>> GetFromConditionAsync(Expression<Func<SellerRequestDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<SellerRequestResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<SellerRequestDBModel>> ProcessQueryAsync(IQueryable<SellerRequestDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
