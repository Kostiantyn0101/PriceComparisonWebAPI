using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    public class ProductCharacteristicUpdateModelValidator : AbstractValidator<ProductCharacteristicUpdateModel>
    {
        public ProductCharacteristicUpdateModelValidator()
        {
            RuleFor(x => x.CharacteristicId).GreaterThan(0).WithMessage("CharacteristicId must be greater than 0");

            RuleFor(x => x)
                .Must(x => x.ValueText != null || x.ValueNumber != null || x.ValueBoolean != null || x.ValueDate != null)
                .WithMessage("At least one value must be specified.");
        }
    }
}
