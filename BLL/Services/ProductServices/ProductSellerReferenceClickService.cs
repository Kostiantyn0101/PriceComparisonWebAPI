using System.Linq.Expressions;
using AutoMapper;
using BLL.Services.SellerServices;
using DLL.Repository;
using Domain.Models.Configuration;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BLL.Services.ProductServices
{
    public class ProductSellerReferenceClickService : IProductSellerReferenceClickService
    {
        private readonly IRepository<ProductSellerReferenceClickDBModel> _repository;
        private readonly IRepository<AuctionClickRateDBModel> _auctionClickRateRepository;
        private readonly IRepository<SellerDBModel> _sellerRepository;
        private readonly SellerAccountConfiguration _accountConfiguration;
        private readonly IMapper _mapper;

        public ProductSellerReferenceClickService(IRepository<ProductSellerReferenceClickDBModel> repository,
            IRepository<AuctionClickRateDBModel> auctionClickRaterepository,
            IRepository<SellerDBModel> sellerRepository,
            IOptions<SellerAccountConfiguration> options,
            IMapper mapper)
        {
            _repository = repository;
            _auctionClickRateRepository = auctionClickRaterepository;
            _sellerRepository = sellerRepository;
            _accountConfiguration = options.Value;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<bool>> ProcessReferenceClick(ProductSellerReferenceClickCreateRequestModel request)
        {
            var model = _mapper.Map<ProductSellerReferenceClickDBModel>(request);
            model.ClickedAt = DateTime.Now;

            var defaultClickRate = _accountConfiguration.DefaultClickRate;
            var currentClickRate = _auctionClickRateRepository.GetQuery()
                    .Where(acr => acr.SellerId == model.SellerId && acr.CategoryId == model.Product.CategoryId)
                    .Select(acr => (decimal?)acr.ClickRate)
                    .FirstOrDefault() ?? defaultClickRate;


            var existingClicks = await _repository.GetFromConditionAsync(x => x.SellerId == model.SellerId && x.UserIp.Equals(model.UserIp) && x.ClickedAt.Date == DateTime.Today);

            if (existingClicks == null || !existingClicks.Any())
            {
                // if there were no clicks yet write off current click rate from account balance
                await WriteOffClickFromTheBalanseAsync(model, currentClickRate);
                //var seller = (await _sellerRepository.GetFromConditionAsync(s => s.Id == model.SellerId)).FirstOrDefault();

                //if (seller != null)
                //{
                //    seller.AccountBalance -= currentClickRate;
                //    _ = await _sellerRepository.UpdateAsync(seller);
                //}
            }
            else if (currentClickRate > defaultClickRate)
            {
                // otherwithe check whether the current click rate is bigger than clicked earlier... 
                var maxExistingClickRate = await _repository.GetQuery()
                    .Where(psrc => psrc.SellerId == model.SellerId && psrc.UserIp.Equals(model.UserIp) && psrc.ClickedAt.Date == DateTime.Today)
                    .Select(psrc => psrc.Seller.AuctionClickRates
                        .Where(acr => acr.CategoryId == psrc.Product.CategoryId)
                        .Select(acr => (decimal?)acr.ClickRate)
                        .FirstOrDefault() ?? _accountConfiguration.DefaultClickRate
                    )
                    .FirstOrDefaultAsync();

                // ...if so, write off the difference from the balance
                if (currentClickRate > maxExistingClickRate)
                {
                    await WriteOffClickFromTheBalanseAsync(model, currentClickRate - maxExistingClickRate);
                }
            }

            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        private async Task WriteOffClickFromTheBalanseAsync(ProductSellerReferenceClickDBModel model, decimal clickRate)
        {
            var seller = (await _sellerRepository.GetFromConditionAsync(s => s.Id == model.SellerId)).FirstOrDefault();

            if (seller != null)
            {
                seller.AccountBalance-= clickRate;
                _ = await _sellerRepository.UpdateAsync(seller);
            }
        }

        public async Task<OperationResultModel<bool>> UpdateAsync(ProductSellerReferenceClickUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<bool>.Failure("ProductSellerReferenceClick record not found.");
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

        public IQueryable<ProductSellerReferenceClickDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ProductSellerReferenceClickResponseModel>> GetFromConditionAsync(Expression<Func<ProductSellerReferenceClickDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<ProductSellerReferenceClickResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<ProductSellerReferenceClickDBModel>> ProcessQueryAsync(IQueryable<ProductSellerReferenceClickDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
