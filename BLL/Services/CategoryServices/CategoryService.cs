using System.Linq.Expressions;
using BLL.Services.MediaServices;
using DLL.Repository;
using Domain.Models.Configuration;
using Domain.Models.DBModels;
using Domain.Models.Exceptions;
using Domain.Models.Response;
using Microsoft.AspNetCore.Http;

namespace BLL.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<CategoryDBModel> _repository;
        private readonly IFileService _fileService;


        public CategoryService(IRepository<CategoryDBModel> repository,
                        IFileService fileService
                        )
        {
            _repository = repository;
            _fileService = fileService;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(CategoryDBModel model)
        {
            return await _repository.CreateAsync(model);
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(CategoryDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public IQueryable<CategoryDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<CategoryDBModel>> GetFromConditionAsync(Expression<Func<CategoryDBModel, bool>> condition)
        {
            return await _repository.GetFromConditionAsync(condition);
        }

        public async Task<IEnumerable<CategoryDBModel>> ProcessQueryAsync(IQueryable<CategoryDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }

        public async Task<OperationDetailsResponseModel> UploadCategoryMediaAsync(int categoryId, string mediaType, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new OperationDetailsResponseModel { IsError = true, Message = AppErrors.General.InternalServerError };

            var category = await GetFromConditionAsync(x => x.Id == categoryId);
            if (category == null || !category.Any())
                return new OperationDetailsResponseModel { IsError = true, Message = AppErrors.General.NotFound };

            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var mediaUrl = await _fileService.SaveImageAsync(file.FileName, stream.ToArray());

                var categoryToUpdate = category.First();
                mediaType = mediaType.ToUpper();

                switch (mediaType)
                {
                    case MediaTypes.Image:
                        categoryToUpdate.ImageUrl = mediaUrl;
                        break;
                    case MediaTypes.Icon:
                        categoryToUpdate.IconUrl = mediaUrl;
                        break;
                    default:
                        return new OperationDetailsResponseModel { IsError = true, Message = AppErrors.General.MediaTypeError };
                }

                return await UpdateAsync(categoryToUpdate);
            }
            catch (InvalidOperationException ex)
            {
                return new OperationDetailsResponseModel { IsError = true, Message = AppErrors.General.InternalServerError, Exception=ex };
            }
        }

    }
}
