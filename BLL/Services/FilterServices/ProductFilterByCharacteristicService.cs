using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Primitives;
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
        private readonly IRepository<FilterCriterionDBModel, int> _filterCriterionRepository;
        private readonly IRepository<CategoryCharacteristicDBModel, CompositeKey<int, int>> _categoryCharacteristicRepository;
        private readonly IRepository<ProductCharacteristicDBModel, CompositeKey<int, int>> _productCharacteristicRepository;
        private readonly IMapper _mapper;

        public ProductFilterByCharacteristicService(
            IRepository<ProductDBModel, int> productRepository,
            IRepository<FilterDBModel, int> filterRepository,
            IRepository<FilterCriterionDBModel, int> filterCriterionRepository,
            IRepository<CategoryCharacteristicDBModel, CompositeKey<int, int>> categoryCharacteristicRepository,
            IRepository<ProductCharacteristicDBModel, CompositeKey<int, int>> productCharacteristicRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _filterRepository = filterRepository;
            _filterCriterionRepository = filterCriterionRepository;
            _categoryCharacteristicRepository = categoryCharacteristicRepository;
            _productCharacteristicRepository = productCharacteristicRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FilterResponseModel>> GetFiltersByProductIdAsync(int productId)
        {
            var product = (await _productRepository.GetFromConditionAsync(p => p.Id == productId)).FirstOrDefault();
            if (product == null || product.CategoryId == null)
                return Enumerable.Empty<FilterResponseModel>();

            int categoryId = product.CategoryId;

            var categoryCharacteristics = await _categoryCharacteristicRepository
                .GetFromConditionAsync(cc => cc.CategoryId == categoryId);
            var characteristicIds = categoryCharacteristics.Select(cc => cc.CharacteristicId).Distinct().ToList();
            if (!characteristicIds.Any())
                return Enumerable.Empty<FilterResponseModel>();

            var filterCriteria = await _filterCriterionRepository
                .GetFromConditionAsync(fc => characteristicIds.Contains(fc.CharacteristicId));
            if (!filterCriteria.Any())
                return Enumerable.Empty<FilterResponseModel>();

            var filterIds = filterCriteria.Select(fc => fc.FilterId).Distinct().ToList();
            var filters = await _filterRepository.GetFromConditionAsync(f => filterIds.Contains(f.Id));

            return _mapper.Map<IEnumerable<FilterResponseModel>>(filters);
        }



        public async Task<IEnumerable<ProductResponseModel>> GetProductsByFilterIdAsync(int filterId)
        {
            var filterCriteria = await GetFilterCriteriaWithCharacteristicAsync(filterId);
            if (!filterCriteria.Any())
                return Enumerable.Empty<ProductResponseModel>();

            var characteristicIds = filterCriteria.Select(fc => fc.CharacteristicId).Distinct().ToList();

            var categoryChars = await _categoryCharacteristicRepository
                .GetFromConditionAsync(cc => characteristicIds.Contains(cc.CharacteristicId));
            var categoryIds = categoryChars.Select(cc => cc.CategoryId).Distinct().ToList();
            if (!categoryIds.Any())
                return Enumerable.Empty<ProductResponseModel>();

            var products = await GetProductsInCategoriesAsync(categoryIds);
            if (!products.Any())
                return Enumerable.Empty<ProductResponseModel>();

            var matchedProducts = new List<ProductDBModel>(products);

            foreach (var criterion in filterCriteria)
            {
                matchedProducts = matchedProducts.Where(product =>
                {
                    var pc = product.ProductCharacteristics
                        .FirstOrDefault(x => x.CharacteristicId == criterion.CharacteristicId);

                    return pc != null && DoesProductCharacteristicMatch(pc, criterion);
                }).ToList();
            }

            return _mapper.Map<IEnumerable<ProductResponseModel>>(matchedProducts);
        }

        private async Task<IEnumerable<FilterCriterionDBModel>> GetFilterCriteriaWithCharacteristicAsync(int filterId)
        {
            var query = _filterCriterionRepository.GetQuery()
                .Include(fc => fc.Characteristic)
                .Where(fc => fc.FilterId == filterId);
            return await _filterCriterionRepository.ProcessQueryAsync(query);
        }

        public async Task<IEnumerable<ProductDBModel>> GetProductsInCategoriesAsync(List<int> categoryIds)
        {
            var query = _productRepository.GetQuery()
                .Include(p => p.ProductCharacteristics)
                    .ThenInclude(pc => pc.Characteristic)
                .Where(p => categoryIds.Contains(p.CategoryId));
            return await _productRepository.ProcessQueryAsync(query);
        }

        private bool DoesProductCharacteristicMatch(ProductCharacteristicDBModel pc, FilterCriterionDBModel criterion)
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
                    "==" => productValue == filterValue,
                    "!=" => productValue != filterValue,
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
