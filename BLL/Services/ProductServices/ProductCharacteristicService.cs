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
    public class ProductCharacteristicService : IProductCharacteristicService
    {
        private readonly IRepository<ProductCharacteristicDBModel, int> _repository;
        private readonly IRepository<ProductDBModel, int> _productRepository;
        private readonly IMapper _mapper;

        public ProductCharacteristicService(IRepository<ProductCharacteristicDBModel, int> repository,
            IRepository<ProductDBModel, int> productRepository,
            IMapper mapper)
        {
            _repository = repository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<bool>> UpdateProductCharacteristicAsync(ProductCharacteristicUpdateRequestModel model)
        {
            if (!model.ProductId.HasValue && !model.BaseProductId.HasValue)
            {
                return OperationResultModel<bool>.Failure("Не вказано жодного ідентифікатора ProductId або BaseProductId.");
            }

            var existingRecords = new List<ProductCharacteristicDBModel>();
            if (model.ProductId.HasValue)
            {
                existingRecords = (await _repository.GetFromConditionAsync(
                    x => x.ProductId == model.ProductId.Value
                )).ToList();
            }
            else
            {
                existingRecords = (await _repository.GetFromConditionAsync(
                    x => x.BaseProductId == model.BaseProductId.Value
                )).ToList();
            }

            foreach (var item in model.Characteristics)
            {
                var record = existingRecords
                    .FirstOrDefault(r => r.CharacteristicId == item.CharacteristicId);

                if (record != null)
                {
                    record.ValueText = item.ValueText;
                    record.ValueNumber = item.ValueNumber;
                    record.ValueBoolean = item.ValueBoolean;
                    record.ValueDate = item.ValueDate;

                    var updateResult = await UpdateAsync(record);
                    if (!updateResult.IsSuccess)
                    {
                        return OperationResultModel<bool>.Failure(
                            updateResult.ErrorMessage!,
                            updateResult.Exception
                        );
                    }
                }
                else
                {
                    var newRecord = _mapper.Map<ProductCharacteristicDBModel>(item);

                    if (model.ProductId.HasValue)
                    {
                        newRecord.ProductId = model.ProductId.Value;
                    }
                    else
                    {
                        newRecord.BaseProductId = model.BaseProductId.Value;
                    }

                    var createResult = await CreateAsync(newRecord);
                    if (!createResult.IsSuccess)
                    {
                        return OperationResultModel<bool>.Failure(
                            createResult.ErrorMessage!,
                            createResult.Exception
                        );
                    }
                }
            }
            return OperationResultModel<bool>.Success(true);
        }


        public async Task<OperationResultModel<ProductCharacteristicDBModel>> CreateAsync(ProductCharacteristicDBModel model)
        {
            var result = await _repository.CreateAsync(model);
            return result.IsSuccess
                ? result
                : OperationResultModel<ProductCharacteristicDBModel>.Failure(result.ErrorMessage!, result.Exception);
        }

        public async Task<OperationResultModel<ProductCharacteristicDBModel>> UpdateAsync(ProductCharacteristicDBModel entity)
        {
            var result = await _repository.UpdateAsync(entity);
            return result.IsSuccess
                ? result
                : OperationResultModel<ProductCharacteristicDBModel>.Failure(result.ErrorMessage!, result.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public IQueryable<ProductCharacteristicDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ProductCharacteristicResponseModel>> GetFromConditionAsync(Expression<Func<ProductCharacteristicDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<ProductCharacteristicResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<ProductCharacteristicDBModel>> ProcessQueryAsync(IQueryable<ProductCharacteristicDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }

        public async Task<IEnumerable<ProductCharacteristicResponseModel>> GetProductCharacteristicsAsync(int productId)
        {
            var baseProductId = await _productRepository.GetQuery()
                .Where(pc => pc.Id == productId)
                .Select(p => p.BaseProductId)
                .FirstOrDefaultAsync();

            var query = _repository.GetQuery()
                                   .Where(pc => pc.ProductId == productId || pc.BaseProductId == baseProductId)
                                   .Include(x => x.Characteristic);

            var dbModels = await _repository.ProcessQueryAsync(query);
            return _mapper.Map<IEnumerable<ProductCharacteristicResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<ProductCharacteristicGroupResponseModel>> GetGroupedProductCharacteristicsAsync(int productId)
        {
            var baseProductId = await _productRepository.GetQuery()
                .Where(pc => pc.Id == productId)
                .Select(p => p.BaseProductId)
                .FirstOrDefaultAsync();

            var query = _repository.GetQuery()
                .Where(pc => pc.ProductId == productId || pc.BaseProductId == baseProductId)
                .Select(pc => new
                {
                    ProductCharacteristic = pc,
                    pc.Characteristic,
                    ProductCategoryId = pc.Product != null ? pc.Product.BaseProduct.CategoryId : pc.BaseProduct != null ? pc.BaseProduct.CategoryId : 0,
                    Group = pc.Characteristic.CharacteristicGroup
                })
                .Select(x => new
                {
                    x.ProductCharacteristic,
                    x.Characteristic,
                    x.ProductCategoryId,
                    x.Group,
                    CategoryGroup = x.Group.CategoryCharacteristicGroups
                        .FirstOrDefault(cg => cg.CategoryId == x.ProductCategoryId)
                });

            var result = query.AsEnumerable()
               .GroupBy(x => new
               {
                   x.Group.Id,
                   x.Group.Title,
                   x.CategoryGroup
               })
               .OrderBy(g => g.Key.CategoryGroup != null ? g.Key.CategoryGroup.GroupDisplayOrder : int.MaxValue)
               .Select(g => new ProductCharacteristicGroupResponseModel
               {
                   CharacteristicGroupId = g.Key.Id,
                   CharacteristicGroupTitle = g.Key.Title,
                   GroupDisplayOrder = g.Key.CategoryGroup?.GroupDisplayOrder ?? 0,
                   ProductCharacteristics = g.Select(x => new ProductCharacteristicResponseModel
                   {
                       ProductId = x.ProductCharacteristic.ProductId,
                       BaseProductId = x.ProductCharacteristic.BaseProductId,
                       CharacteristicId = x.ProductCharacteristic.CharacteristicId,
                       CharacteristicTitle = x.Characteristic.Title,
                       CharacteristicUnit = x.Characteristic.Unit,
                       CharacteristicDataType = x.Characteristic.DataType,
                       ValueText = x.ProductCharacteristic.ValueText,
                       ValueNumber = x.ProductCharacteristic.ValueNumber,
                       ValueBoolean = x.ProductCharacteristic.ValueBoolean,
                       ValueDate = x.ProductCharacteristic.ValueDate
                   })
               })
               .ToList();

            return result;
        }

        public async Task<IEnumerable<ProductCharacteristicGroupResponseModel>> GetShortGroupedProductCharacteristicsAsync(int productId)
        {
            var baseProductId = await _productRepository.GetQuery()
                .Where(pc => pc.Id == productId)
                .Select(p => p.BaseProductId)
                .FirstOrDefaultAsync();

            var query = _repository.GetQuery()
                .Where(pc => pc.Characteristic.IncludeInShortDescription && (pc.ProductId == productId || pc.BaseProductId == baseProductId))
                .Select(pc => new
                {
                    ProductCharacteristic = pc,
                    pc.Characteristic,
                    ProductCategoryId = pc.Product != null ? pc.Product.BaseProduct.CategoryId : pc.BaseProduct != null ? pc.BaseProduct.CategoryId : 0,
                    Group = pc.Characteristic.CharacteristicGroup
                })
                .Select(x => new
                {
                    x.ProductCharacteristic,
                    x.Characteristic,
                    x.ProductCategoryId,
                    x.Group,
                    CategoryGroup = x.Group.CategoryCharacteristicGroups
                        .FirstOrDefault(cg => cg.CategoryId == x.ProductCategoryId)
                });

            var result = query.AsEnumerable()
               .GroupBy(x => new
               {
                   x.Group.Id,
                   x.Group.Title,
                   x.CategoryGroup
               })
               .OrderBy(g => g.Key.CategoryGroup != null ? g.Key.CategoryGroup.GroupDisplayOrder : int.MaxValue)
               .Select(g => new ProductCharacteristicGroupResponseModel
               {
                   CharacteristicGroupId = g.Key.Id,
                   CharacteristicGroupTitle = g.Key.Title,
                   GroupDisplayOrder = g.Key.CategoryGroup?.GroupDisplayOrder ?? 0,
                   ProductCharacteristics = g.Select(x => new ProductCharacteristicResponseModel
                   {
                       ProductId = x.ProductCharacteristic.ProductId ?? 0,
                       BaseProductId = x.ProductCharacteristic.BaseProductId,
                       CharacteristicId = x.ProductCharacteristic.CharacteristicId,
                       CharacteristicTitle = x.Characteristic.Title,
                       CharacteristicUnit = x.Characteristic.Unit,
                       CharacteristicDataType = x.Characteristic.DataType,
                       ValueText = x.ProductCharacteristic.ValueText,
                       ValueNumber = x.ProductCharacteristic.ValueNumber,
                       ValueBoolean = x.ProductCharacteristic.ValueBoolean,
                       ValueDate = x.ProductCharacteristic.ValueDate
                   })
               })
               .ToList();

            return result;
        }
    }
}
