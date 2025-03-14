using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    public class ProductUpdateRequestModelValidator : AbstractValidator<ProductUpdateRequestModel>
    {
        public ProductUpdateRequestModelValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Product id must be greater than 0");

            RuleFor(x => x.GTIN)
                .MaximumLength(15).WithMessage("Product GTIN length must be less than 15 characters")
                .When(x => x.GTIN != null);

            RuleFor(x => x.UPC)
                .MaximumLength(15).WithMessage("Product UPC length must be less than 15 characters")
                .When(x => x.UPC != null);

            RuleFor(x => x.ModelNumber)
                .MaximumLength(255).WithMessage("Product model number length must be less than 255 characters")
                .When(x => x.ModelNumber != null);

            RuleFor(x => x.BaseProductId)
                .GreaterThan(0).WithMessage("Base product id must be greater than 0");

            RuleFor(x => x.ColorId)
                .GreaterThan(0).WithMessage("Color id must be greater than 0")
                .When(x => x.ColorId.HasValue);

            RuleFor(x => x.VariantName)
                .MaximumLength(200).WithMessage("Variant name length must be less than 200 characters")
                .When(x => !string.IsNullOrEmpty(x.VariantName));
        }
    }
}
