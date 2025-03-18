using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    public class ProductCharacteristicCreateRequestModelListValidator : AbstractValidator<IEnumerable<ProductCharacteristicCreateRequestModel>>
    {
        public ProductCharacteristicCreateRequestModelListValidator()
        {
            RuleForEach(items => items)
           .SetValidator(new ProductCharacteristicCreateRequestModelValidator());
        }
    }
}
