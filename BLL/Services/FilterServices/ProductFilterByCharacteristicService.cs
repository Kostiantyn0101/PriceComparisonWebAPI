using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Response;
using Domain.Models.Response.Filters;
using Domain.Models.Response.Products;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BLL.Services.FilterServices
{
    public class ProductFilterByCharacteristicService : IProductFilterByCharacteristicService
    {
        private readonly IRepository<ProductDBModel, int> _productRepository;
        private readonly IRepository<FilterDBModel, int> _filterRepository;
        private readonly IMapper _mapper;

        public ProductFilterByCharacteristicService(
            IRepository<ProductDBModel, int> productRepository,
            IRepository<FilterDBModel, int> filterRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _filterRepository = filterRepository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<IEnumerable<FilterResponseModel>>> GetFiltersByProductIdAsync(int productId)
        {
            var productQuery = _productRepository.GetQuery()
                .Include(p => p.ProductCharacteristics).ThenInclude(pc => pc.Characteristic);
            var product = (await _productRepository.ProcessQueryAsync(productQuery))
                          .FirstOrDefault(p => p.Id == productId);
            if (product == null || product.CategoryId == null)
            {
                return OperationResultModel<IEnumerable<FilterResponseModel>>.Failure("Product not found or has no category", null);
            }    

            int categoryId = product.CategoryId;

            var filters = await _filterRepository.GetFromConditionAsync(f =>
                                f.CategoryId == categoryId && f.DisplayOnProduct);
            if (!filters.Any())
            {
                return OperationResultModel<IEnumerable<FilterResponseModel>>.Failure("No filters found for the category", null);
            }

            var matchedFilters = filters.Where(filter =>
            {
                var pc = product.ProductCharacteristics
                    .FirstOrDefault(x => x.CharacteristicId == filter.CharacteristicId);
                return pc != null && DoesProductCharacteristicMatch(pc, filter);
            }).ToList();

            var mappedFilters = _mapper.Map<IEnumerable<FilterResponseModel>>(matchedFilters);
            return OperationResultModel<IEnumerable<FilterResponseModel>>.Success(mappedFilters);
        }

        public async Task<OperationResultModel<IEnumerable<FilterResponseModel>>> GetFiltersByCategoryIdAsync(int categoryId)
        {
            var filters = await _filterRepository.GetFromConditionAsync(f =>
                f.CategoryId == categoryId && f.DisplayOnProduct);
            if (!filters.Any())
            {
                return OperationResultModel<IEnumerable<FilterResponseModel>>.Failure("No filters found for the specified category", null);
            }
            var mappedFilters = _mapper.Map<IEnumerable<FilterResponseModel>>(filters);
            return OperationResultModel<IEnumerable<FilterResponseModel>>.Success(mappedFilters);
        }

        public async Task<OperationResultModel<IEnumerable<ProductResponseModel>>> GetProductsByFilterIdsAsync(int[] filterIds)
        {
            var filters = await _filterRepository.GetFromConditionAsync(f => filterIds.Contains(f.Id));
            if (!filters.Any())
                return OperationResultModel<IEnumerable<ProductResponseModel>>.Failure("No filters found", null);

            var matchedProducts = await GetMatchedProducts(filters);
            return OperationResultModel<IEnumerable<ProductResponseModel>>.Success(_mapper.Map<IEnumerable<ProductResponseModel>>(matchedProducts));
        }

        public async Task<OperationResultModel<IEnumerable<ProductResponseModel>>> GetProductsByFilterIdAsync(int filterId)
        {
            var filter = (await _filterRepository.GetFromConditionAsync(f => f.Id == filterId)).FirstOrDefault();
            if (filter == null)
                return OperationResultModel<IEnumerable<ProductResponseModel>>.Failure("Filter not found", null);

            var matchedProducts = await GetMatchedProducts(new List<FilterDBModel> { filter });
            return OperationResultModel<IEnumerable<ProductResponseModel>>.Success(_mapper.Map<IEnumerable<ProductResponseModel>>(matchedProducts));
        }


        private async Task<IEnumerable<ProductDBModel>> GetMatchedProducts(IEnumerable<FilterDBModel> filters)
        {
            var categoryIds = filters.Select(f => f.CategoryId).Distinct().ToList();
            if (!categoryIds.Any())
                return Enumerable.Empty<ProductDBModel>();

            var products = await GetProductsInCategoriesAsync(categoryIds);
            if (!products.Any())
                return Enumerable.Empty<ProductDBModel>();

            var matchedProducts = products.Where(product =>
            {
                foreach (var filter in filters)
                {
                    var pc = product.ProductCharacteristics?.FirstOrDefault(x => x.CharacteristicId == filter.CharacteristicId);
                    if (pc == null || !DoesProductCharacteristicMatch(pc, filter))
                        return false;
                }
                return true;
            }).ToList();

            return matchedProducts;
        }

        private async Task<IEnumerable<ProductDBModel>> GetProductsInCategoriesAsync(List<int> categoryIds)
        {
            var query = _productRepository.GetQuery()
                .Include(p => p.ProductCharacteristics)
                    .ThenInclude(pc => pc.Characteristic)
                .Where(p => categoryIds.Contains(p.CategoryId));
            return await _productRepository.ProcessQueryAsync(query);
        }

        private bool DoesProductCharacteristicMatch(ProductCharacteristicDBModel pc, FilterDBModel criterion)
        {
            string dataType = criterion.Characteristic.DataType?.ToLowerInvariant() ?? "string";
            switch (dataType)
            {
                case "decimal":
                    if (!pc.ValueNumber.HasValue) return false;
                    if (decimal.TryParse(criterion.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal critDecimal))
                    {
                        return CompareDecimal(pc.ValueNumber.Value, critDecimal, criterion.Operator);
                    }
                    return false;
                case "bool":
                    if (!pc.ValueBoolean.HasValue) return false;
                    if (bool.TryParse(criterion.Value, out bool critBool))
                    {
                        return CompareBool(pc.ValueBoolean.Value, critBool, criterion.Operator);
                    }
                    return false;
                case "datetime":
                    if (!pc.ValueDate.HasValue) return false;
                    if (DateTime.TryParse(criterion.Value, out DateTime critDate))
                    {
                        return CompareDate(pc.ValueDate.Value, critDate, criterion.Operator);
                    }
                    return false;
                case "string":
                default:
                    if (string.IsNullOrEmpty(pc.ValueText)) return false;
                    return CompareString(pc.ValueText, criterion.Value.ToString(), criterion.Operator);
            }
        }

        private bool CompareDecimal(decimal productValue, decimal filterValue, string _operator)
        {
            return _operator switch
            {
                "==" => productValue == filterValue,
                "!=" => productValue != filterValue,
                ">" => productValue > filterValue,
                ">=" => productValue >= filterValue,
                "<" => productValue < filterValue,
                "<=" => productValue <= filterValue,
                _ => false,
            };
        }

        private bool CompareBool(bool productValue, bool filterValue, string _operator)
        {
            return _operator switch
            {
                "==" => productValue == filterValue,
                "!=" => productValue != filterValue,
                _ => false,
            };
        }

        private bool CompareDate(DateTime productValue, DateTime filterValue, string _operator)
        {
            return _operator switch
            {
                "==" => productValue == filterValue,
                "!=" => productValue != filterValue,
                ">" => productValue > filterValue,
                ">=" => productValue >= filterValue,
                "<" => productValue < filterValue,
                "<=" => productValue <= filterValue,
                _ => false,
            };
        }

        private bool CompareString(string productValue, string filterValue, string _operator)
        {
            productValue = productValue.ToLowerInvariant();
            filterValue = filterValue.ToLowerInvariant();
            if (productValue.Contains("мп") && filterValue.Contains("мп"))
            {
                decimal productNumeric = ExtractDecimal(productValue);
                decimal filterNumeric = ExtractDecimal(filterValue);

                return _operator switch
                {
                    "==" => productNumeric == filterNumeric,
                    "!=" => productNumeric != filterNumeric,
                    ">" => productNumeric > filterNumeric,
                    ">=" => productNumeric >= filterNumeric,
                    "<" => productNumeric < filterNumeric,
                    "<=" => productNumeric <= filterNumeric,
                    _ => false,
                };
            }
            else
            {
                return _operator switch
                {
                    "==" => productValue.Contains(filterValue),
                    "!=" => !productValue.Contains(filterValue),
                    "contains" => productValue.Contains(filterValue),
                    _ => false,
                };
            }
        }

        private decimal ExtractDecimal(string text)
        {
            var match = Regex.Match(text, @"(\d+(?:[.,]\d+)?)");
            if (match.Success)
            {
                string numberStr = match.Groups[1].Value.Replace(',', '.');
                if (decimal.TryParse(numberStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
                {
                    return result;
                }
            }
            return 0;
        }
    }
}
