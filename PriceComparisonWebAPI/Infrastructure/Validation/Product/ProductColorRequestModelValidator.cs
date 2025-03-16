using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    public class ProductColorRequestModelValidator : AbstractValidator<ProductColorRequestModel>
    {
        public ProductColorRequestModelValidator()
        {
            RuleFor(x => x.BaseProductId)
                   .GreaterThan(0).WithMessage("BaseProductId must be greater than 0.");

            RuleFor(x => x.ProductGroupId)
                   .GreaterThan(0).WithMessage("ProductGroupId must be greater than 0.");
        }
    }
}
