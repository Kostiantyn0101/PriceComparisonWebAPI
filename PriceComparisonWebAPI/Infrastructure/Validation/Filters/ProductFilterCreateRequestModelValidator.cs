using Domain.Models.Request.Filters;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Filters
{
    public class ProductFilterCreateRequestModelValidator : AbstractValidator<ProductFilterCreateRequestModel>
    {
        public ProductFilterCreateRequestModelValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");

            RuleFor(x => x.FilterId)
                .GreaterThan(0).WithMessage("FilterId must be greater than 0.");
        }
    }
}
