using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation
{
    public class ProductImageSetPrimaryRequestModelValidator : AbstractValidator<ProductImageSetPrimaryRequestModel>
    {
        public ProductImageSetPrimaryRequestModelValidator()
        {
            RuleFor(x => x.ProductImageId)
                .GreaterThan(0).WithMessage("Product image id must be greater than 0");
        }
    }
}
