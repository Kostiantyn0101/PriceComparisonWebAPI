using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation
{
    public class ProductImageDeleteRequestModelValidator : AbstractValidator<ProductImageDeleteRequestModel>
    {
        public ProductImageDeleteRequestModelValidator()
        {
            RuleFor(x => x.ProductImageIds)
                .NotEmpty().WithMessage("Product image ids list is required");

            RuleForEach(x => x.ProductImageIds)
                .GreaterThan(0).WithMessage("Each product image id must be greater than 0");
        }
    }
}
