using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    public class ProductCharacteristicCreateRequestModelValidator : AbstractValidator<ProductCharacteristicCreateRequestModel>
    {
        public ProductCharacteristicCreateRequestModelValidator()
        {
            // Either BaseProductId or ProductId must be provided
            RuleFor(model => model)
                .Must(model => model.BaseProductId.HasValue || model.ProductId.HasValue)
                .WithMessage("Either BaseProductId or ProductId must be provided");

            // If BaseProductId is provided, it must be greater than 0
            RuleFor(model => model.BaseProductId.Value)
                .GreaterThan(0)
                .WithMessage("BaseProductId must be greater than 0")
                .When(model => model.BaseProductId.HasValue);

            // If ProductId is provided, it must be greater than 0
            RuleFor(model => model.ProductId.Value)
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than 0")
                .When(model => model.ProductId.HasValue);
            
            // CharacteristicId must be greater than 0
            RuleFor(model => model.CharacteristicId)
                .GreaterThan(0)
                .WithMessage("CharacteristicId must be greater than 0");

            // At least one value property must be provided
            RuleFor(model => model)
                .Must(model =>
                    !string.IsNullOrEmpty(model.ValueText) ||
                    model.ValueNumber.HasValue ||
                    model.ValueBoolean.HasValue ||
                    model.ValueDate.HasValue)
                .WithMessage("At least one of ValueText, ValueNumber, ValueBoolean, or ValueDate must be provided");
        }
    }
}
