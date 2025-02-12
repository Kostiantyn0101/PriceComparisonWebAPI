using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation
{
    public class ProductImageCreateRequestModelValidator : AbstractValidator<ProductImageCreateRequestModel>
    {
        public ProductImageCreateRequestModelValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product id must be greater than 0");

            RuleFor(x => x.Images)
                .NotEmpty().WithMessage("At least one image is required");

            RuleForEach(x => x.Images)
                .ImageFileRules();
        }
    }
}
