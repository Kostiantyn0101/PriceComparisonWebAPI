using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    public class ProductCharacteristicUpdateRequestModelListValidator : AbstractValidator<IEnumerable<ProductCharacteristicUpdateRequestModel>>
    {
        public ProductCharacteristicUpdateRequestModelListValidator()
        {
            RuleForEach(items => items)
           .SetValidator(new ProductCharacteristicUpdateRequestModelValidator());
        }
    }
}
