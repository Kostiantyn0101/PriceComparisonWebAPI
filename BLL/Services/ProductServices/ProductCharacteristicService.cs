using AutoMapper;
using Azure.Core;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Exceptions;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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


        public async Task<OperationResultModel<IEnumerable<ProductCharacteristicResponseModel>>> CreateProductCharacteristicsAsync(IEnumerable<ProductCharacteristicCreateRequestModel> models)
        {
            var dbModels = _mapper.Map<IEnumerable<ProductCharacteristicDBModel>>(models);
            var responseModels = new List<ProductCharacteristicResponseModel>();
            var errorMessages = new List<string>();
            var exceptions = new List<Exception>();

            int index = 1;
            foreach (var model in dbModels)
            {
                var result = await _repository.CreateAsync(model);

                if (result.IsSuccess)
                {
                    responseModels.Add(_mapper.Map<ProductCharacteristicResponseModel>(model));
                }
                else
                {
                    errorMessages.Add($"Error when creating item  #{index}: {result.ErrorMessage}");

                    if (result.Exception != null)
                    {
                        exceptions.Add(result.Exception);
                    }
                }

                index++;
            }

            if (!errorMessages.Any())
            {
                return OperationResultModel<IEnumerable<ProductCharacteristicResponseModel>>.Success(responseModels);
            }
            else
            {
                var combinedErrorMessage = string.Join("; ", errorMessages);
                var aggregateException = exceptions.Any() ? new AggregateException(exceptions) : null;
                return OperationResultModel<IEnumerable<ProductCharacteristicResponseModel>>.Failure(combinedErrorMessage, aggregateException);
            }
        }


        public async Task<OperationResultModel<IEnumerable<ProductCharacteristicResponseModel>>> UpdateProductCharacteristicsAsync(IEnumerable<ProductCharacteristicUpdateRequestModel> requests)
        {
            var responseModels = new List<ProductCharacteristicResponseModel>();
            var errorMessages = new List<string>();
            var exceptions = new List<Exception>();

            foreach (var request in requests)
            {
                var existing = (await _repository.GetFromConditionAsync(pc => pc.Id == request.Id)).FirstOrDefault();
                if (existing == null)
                {
                    errorMessages.Add($"Entity with ID {request.Id} not found.");
                    continue;
                }

                _mapper.Map(request, existing);
                var result = await _repository.UpdateAsync(existing);

                if (result.IsSuccess)
                {
                    responseModels.Add(_mapper.Map<ProductCharacteristicResponseModel>(existing));
                }
                else
                {
                    errorMessages.Add(result.ErrorMessage!);
                    if (result.Exception != null)
                    {
                        exceptions.Add(result.Exception);
                    }
                }
            }

            if (!errorMessages.Any())
            {
                return OperationResultModel<IEnumerable<ProductCharacteristicResponseModel>>.Success(responseModels);
            }
            else
            {
                var combinedErrorMessage = string.Join("; ", errorMessages);
                var aggregateException = exceptions.Any() ? new AggregateException(exceptions) : null;
                return OperationResultModel<IEnumerable<ProductCharacteristicResponseModel>>.Failure(combinedErrorMessage, aggregateException);
            }
        }


        public async Task<OperationResultModel<ProductCharacteristicResponseModel>> CreateAsync(ProductCharacteristicCreateRequestModel request)
        {
            var dbModel = _mapper.Map<ProductCharacteristicDBModel>(request);
            var result = await _repository.CreateAsync(dbModel);
            return result.IsSuccess
                ? OperationResultModel<ProductCharacteristicResponseModel>.Success(_mapper.Map<ProductCharacteristicResponseModel>(dbModel))
                : OperationResultModel<ProductCharacteristicResponseModel>.Failure(result.ErrorMessage!, result.Exception);
        }


        public async Task<OperationResultModel<ProductCharacteristicResponseModel>> UpdateAsync(ProductCharacteristicUpdateRequestModel request)
        {
            var existing = (await _repository.GetFromConditionAsync(pc => pc.Id == request.Id)).FirstOrDefault();

            if (existing == null)
            {
                return OperationResultModel<ProductCharacteristicResponseModel>.Failure(AppErrors.General.NotFound);
            }

            _mapper.Map(request, existing);

            var result = await _repository.UpdateAsync(existing);
            return result.IsSuccess
                ? OperationResultModel<ProductCharacteristicResponseModel>.Success(_mapper.Map<ProductCharacteristicResponseModel>(existing))
                : OperationResultModel<ProductCharacteristicResponseModel>.Failure(result.ErrorMessage!, result.Exception);
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


        public async Task<IEnumerable<ProductCharacteristicResponseModel>> GetBaseProductCharacteristicsAsync(int baseProductId)
        {
            var query = _repository.GetQuery()
                                   .Where(pc => pc.BaseProductId == baseProductId)
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
