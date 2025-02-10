using System.Linq.Expressions;
using AutoMapper;
using BLL.Services.MediaServices;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Categories;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductService
{
    public class ProductImageService : IProductImageService
    {
        private readonly IRepository<ProductImageDBModel> _repository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public ProductImageService(IRepository<ProductImageDBModel> repository, IFileService fileService, IMapper mapper)
        {
            _repository = repository;
            _fileService = fileService;
            _mapper = mapper;
        }

        public IQueryable<ProductImageDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ProductImageResponseModel>> GetFromConditionAsync(Expression<Func<ProductImageDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<ProductImageResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<ProductImageDBModel>> ProcessQueryAsync(IQueryable<ProductImageDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }


        public async Task<OperationResultModel<bool>> AddAsync(ProductImageCreateRequestModel model)
        {
            foreach (var image in model.Images)
            {
                var saveResult = await _fileService.SaveImageAsync(image);
                if (!saveResult.IsSuccess)
                {
                    return OperationResultModel<bool>.Failure(saveResult.ErrorMessage, saveResult.Exception);
                }

                var productImage = new ProductImageDBModel
                {
                    ProductId = model.ProductId,
                    ImageUrl = saveResult.Data,
                    IsPrimary = false
                };

                var createResult = await _repository.CreateAsync(productImage);
                if (createResult.IsError)
                {
                    return OperationResultModel<bool>.Failure(createResult.Message, createResult.Exception);
                }
            }
            return OperationResultModel<bool>.Success(true);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(ProductImageDeleteRequestModel model)
        {
            foreach (var id in model.ProductImageIds)
            {
                var images = await GetFromConditionAsync(x => x.Id == id);
                var productImage = images.FirstOrDefault();
                if (productImage == null)
                    continue;

                if (!string.IsNullOrEmpty(productImage.ImageUrl))
                {
                    var deleteFileResult = await _fileService.DeleteImageAsync(productImage.ImageUrl);
                }

                var deleteRecordResult = await _repository.DeleteAsync(id);
                if (deleteRecordResult.IsError)
                {
                    return OperationResultModel<bool>.Failure(deleteRecordResult.Message, deleteRecordResult.Exception);
                }
            }
            return OperationResultModel<bool>.Success(true);
        }

        public async Task<OperationResultModel<bool>> SetPrimaryImageAsync(ProductImageSetPrimaryRequestModel model)
        {
            var images = await GetFromConditionAsync(x => x.Id == model.ProductImageId);
            var productImage = images.FirstOrDefault();
            if (productImage == null)
            {
                return OperationResultModel<bool>.Failure("Entity not found");
            }

            var allImages = await _repository.GetFromConditionAsync(x => x.ProductId == productImage.ProductId);
            foreach (var img in allImages)
            {
                bool shouldBePrimary = img.Id == model.ProductImageId;
                if (img.IsPrimary != shouldBePrimary)
                {
                    img.IsPrimary = shouldBePrimary;
                    var updateResult = await _repository.UpdateAsync(img);
                    if (updateResult.IsError)
                    {
                        return OperationResultModel<bool>.Failure(updateResult.Message, updateResult.Exception);
                    }
                }
            }
            return OperationResultModel<bool>.Success(true);
        }
    }
}
