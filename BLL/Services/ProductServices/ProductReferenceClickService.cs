using AutoMapper;
using DLL.Repository;
using Domain.Models.Configuration;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IO.Pipelines;
using System.Linq.Expressions;

namespace BLL.Services.ProductServices
{
    public class ProductReferenceClickService : IProductReferenceClickService
    {
        private readonly IRepository<ProductReferenceClickDBModel, int> _repository;
        private readonly IRepository<AuctionClickRateDBModel, int> _auctionClickRateRepository;
        private readonly IRepository<SellerDBModel, int> _sellerRepository;
        private readonly IRepository<ProductDBModel, int> _productRepository;
        private readonly SellerAccountConfiguration _accountConfiguration;
        private readonly IMapper _mapper;

        public ProductReferenceClickService(IRepository<ProductReferenceClickDBModel, int> repository,
            IRepository<AuctionClickRateDBModel, int> auctionClickRaterepository,
            IRepository<SellerDBModel, int> sellerRepository,
            IRepository<ProductDBModel, int> productRepository,
            IOptions<SellerAccountConfiguration> options,
            IMapper mapper)
        {
            _repository = repository;
            _auctionClickRateRepository = auctionClickRaterepository;
            _sellerRepository = sellerRepository;
            _productRepository = productRepository;
            _accountConfiguration = options.Value;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<bool>> ProcessReferenceClick(ProductSellerReferenceClickCreateRequestModel request)
        {
            var model = _mapper.Map<ProductReferenceClickDBModel>(request);
            model.ClickedAt = DateTime.Now;

            var product = await _productRepository.GetQuery()
                .Include(p => p.BaseProduct)
                .Where(p => p.Id == model.ProductId)
                .FirstOrDefaultAsync();

            var categoryId = product?.BaseProduct.CategoryId ?? 0;

            var defaultClickRate = _accountConfiguration.DefaultClickRate;
            var currentClickRate = await _auctionClickRateRepository.GetQuery()
                    .Where(acr => acr.SellerId == model.SellerId && acr.CategoryId == categoryId)
                    .Select(acr => (decimal?)acr.ClickRate)
                    .FirstOrDefaultAsync() ?? defaultClickRate;

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var existingClicks = await _repository.GetFromConditionAsync(x =>
                x.SellerId == model.SellerId &&
                x.UserIp.Equals(model.UserIp) &&
                x.ClickedAt >= today &&
                x.ClickedAt < tomorrow);

            if (existingClicks == null || !existingClicks.Any())
            {
                // if there were no clicks yet write off current click rate from account balance.
                await WriteOffClickFromTheBalanseAsync(model, currentClickRate);
                model.ClickRate = currentClickRate;
            }
            else if (currentClickRate > defaultClickRate)
            {
                // otherwithe check whether the current click rate is bigger than clicked earlier... 
                var maxExistingClickRateRecord = existingClicks.MaxBy(x => x.ClickRate);
                var maxExistingClickRate = maxExistingClickRateRecord?.ClickRate ?? 0;

                if (currentClickRate > maxExistingClickRate)
                {
                    await WriteOffClickFromTheBalanseAsync(model, currentClickRate - maxExistingClickRate);     // write off difference from the balance

                    maxExistingClickRateRecord!.ClickRate = 0;                                              // clear old click rate
                    _ = await _repository.UpdateAsync(maxExistingClickRateRecord);

                    model.ClickRate = currentClickRate;                                                     // write new click rate    
                }

            }

            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<List<ProductSellerReferenceClickResponseModel>> GetReferenceClickStatisticAsync(ProductSellerReferenceClickStaisticRequestModel request)
        {
            var query = _repository.GetQuery()
                .Where(rc => rc.ClickedAt >= request.StartDate && rc.ClickedAt <= request.EndDate && rc.SellerId == request.SellerId)
                .Select(rc => new ProductSellerReferenceClickResponseModel()
                {
                    ClickedAt = rc.ClickedAt,
                    ProductId = rc.ProductId,
                    ProductName = rc.Product.BaseProduct.Title + " " + rc.Product.ProductGroup.Name,
                    CategoryName = rc.Product.BaseProduct.Category.Title,
                    ClickRate = rc.ClickRate,
                    SellerId = rc.SellerId,
                    Id = rc.Id,
                    SellerProductReference = rc.Seller.SellerProductDetails
                        .Where(pd => pd.ProductId == rc.ProductId && pd.ProductId == rc.ProductId)
                        .Select(pd => pd.ProductStoreUrl)
                        .FirstOrDefault() ?? string.Empty
                });

            var result = await query.ToListAsync();

            return result;
        }


        private async Task WriteOffClickFromTheBalanseAsync(ProductReferenceClickDBModel model, decimal clickRate)
        {
            var seller = (await _sellerRepository.GetFromConditionAsync(s => s.Id == model.SellerId)).FirstOrDefault();

            if (seller != null)
            {
                seller.AccountBalance -= clickRate;
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
            return repoResult.IsSuccess
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var repoResult = await _repository.DeleteAsync(id);
            return repoResult.IsSuccess
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public IQueryable<ProductReferenceClickDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ProductSellerReferenceClickResponseModel>> GetFromConditionAsync(Expression<Func<ProductReferenceClickDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<ProductSellerReferenceClickResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<ProductReferenceClickDBModel>> ProcessQueryAsync(IQueryable<ProductReferenceClickDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }


    }
}

//var maxExistingClickRate2 = await _repository.GetQuery()
//    .Where(psrc => psrc.SellerId == model.SellerId && psrc.UserIp.Equals(model.UserIp) && psrc.ClickedAt >= today && psrc.ClickedAt < tomorrow)
//    .Select(psrc => psrc.Seller.AuctionClickRates
//        .Where(acr => acr.CategoryId == psrc.Product.CategoryId)
//        .Select(acr => (decimal?)acr.ClickRate)
//        .FirstOrDefault() ?? _accountConfiguration.DefaultClickRate
//    )
//    .FirstOrDefaultAsync();

// ...if so, write off the difference from the balance