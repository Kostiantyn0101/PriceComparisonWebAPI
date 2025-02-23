using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    public class ProductRequestModelValidator : AbstractValidator<ProductRequestModel>
    {
        public ProductRequestModelValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Product id must be greater than 0");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Product title is required.")
                .MaximumLength(200).WithMessage("Product title length must be less than 200 characters");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Product brand is required.")
                .MaximumLength(255).WithMessage("Product brand length must be less than 255 characters");

            RuleFor(x => x.ModelNumber)
                .MaximumLength(255).WithMessage("Product model number length must be less than 255 characters")
                .When(x => x.ModelNumber != null);

            RuleFor(x => x.GTIN)
                .MaximumLength(15).WithMessage("Product GTIN length must be less than 15 characters")
                .When(x => x.GTIN != null);

            RuleFor(x => x.UPC)
                .MaximumLength(15).WithMessage("Product UPC length must be less than 15 characters")
                .When(x => x.UPC != null);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category id must be greater than 0");

            RuleFor(x => x.Description)
                .MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("Description length must be less than 2000 characters");
        }
    }
}
