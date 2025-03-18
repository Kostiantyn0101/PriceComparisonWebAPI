using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    public class ProductCharacteristicUpdateRequestModelValidator : AbstractValidator<ProductCharacteristicUpdateRequestModel>
    {
        public ProductCharacteristicUpdateRequestModelValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("ProductId must be greater than 0.");

            RuleForEach(x => x.Characteristics).SetValidator(new ProductCharacteristicUpdateModelValidator());
        }
    }
}
