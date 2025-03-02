using System.Linq.Expressions;
using AutoMapper;
using BLL.Services.MediaServices;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Response;
using Domain.Models.Request.Categories;
using Domain.Models.Response.Categories;

namespace BLL.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<CategoryDBModel> _repository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;


        public CategoryService(IRepository<CategoryDBModel> repository, IFileService fileService, IMapper mapper)
        {
            _repository = repository;
            _fileService = fileService;
            _mapper = mapper;
        }


        public async Task<OperationResultModel<CategoryDBModel>> CreateAsync(CategoryCreateRequestModel model)
        {
            var dbModel = _mapper.Map<CategoryDBModel>(model);

            if (model.Image != null)
            {
                var result = await _fileService.SaveImageAsync(model.Image);

                if (result.IsSuccess)
                {
                    dbModel.ImageUrl = result.Data;
                }
                else
                {
                    return OperationResultModel<CategoryDBModel>.Failure("Image save error");
                }
            }

            if (model.Icon != null)
            {
                var result = await _fileService.SaveImageAsync(model.Icon);

                if (result.IsSuccess)
                {
                    dbModel.IconUrl = result.Data;
                }
                else
                {
                    return OperationResultModel<CategoryDBModel>.Failure("Image save error");
                }
            }

            return await _repository.CreateAsync(dbModel);
        }


        public async Task<OperationDetailsResponseModel> UpdateAsync(CategoryUpdateRequestModel entity)
        {
            var dbModel = (await _repository.GetFromConditionAsync(x => x.Id == entity.Id)).FirstOrDefault();
            if (dbModel == null)
            {
                return new OperationDetailsResponseModel { IsError = true, Message = "Entity not found" };
            }

            dbModel = _mapper.Map(entity, dbModel);


            if ((entity.NewIcon != null || entity.DeleteCurrentIcon) && dbModel.IconUrl != null)
            {
                var deleteResult = await _fileService.DeleteImageAsync(dbModel.IconUrl);
                if (deleteResult.IsSuccess)
                {
                    dbModel.IconUrl = null;
                }
            }

            if (entity.NewIcon != null)
            {
                var saveResult = await _fileService.SaveImageAsync(entity.NewIcon);
                if (saveResult.IsSuccess)
                {
                    dbModel.IconUrl = saveResult.Data;
                }
                else
                {
                    return new OperationDetailsResponseModel { IsError = true, Message = "Image save error" };
                }
            }

            if ((entity.NewImage != null || entity.DeleteCurrentImage) && dbModel.ImageUrl != null)
            {
                var deleteResult = await _fileService.DeleteImageAsync(dbModel.ImageUrl);
                if (deleteResult.IsSuccess)
                {
                    dbModel.ImageUrl = null;
                }
            }

            if (entity.NewImage != null)
            {
                var saveResult = await _fileService.SaveImageAsync(entity.NewImage);
                if (saveResult.IsSuccess)
                {
                    dbModel.ImageUrl = saveResult.Data;
                }
                else
                {
                    return new OperationDetailsResponseModel { IsError = true, Message = "Image save error" };
                }
            }

            return await _repository.UpdateAsync(dbModel);
        }


        public async Task<OperationDetailsResponseModel> DeleteAsync(int id)
        {
            var dbModel = (await _repository.GetFromConditionAsync(x => x.Id == id)).FirstOrDefault();

            if (dbModel != null && dbModel.IconUrl != null)
            {
                await _fileService.DeleteImageAsync(dbModel.IconUrl);
            }
            if (dbModel != null && dbModel.ImageUrl != null)
            {
                await _fileService.DeleteImageAsync(dbModel.ImageUrl);
            }

            return await _repository.DeleteAsync(id);
        }

        public IQueryable<CategoryDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<CategoryResponseModel>> GetFromConditionAsync(Expression<Func<CategoryDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<CategoryResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<CategoryDBModel>> ProcessQueryAsync(IQueryable<CategoryDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
