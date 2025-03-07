using Domain.Models.Request.Filters;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Filters
{
    public class ProductFilterUpdateRequestModelValidator : AbstractValidator<ProductFilterUpdateRequestModel>
    {
        public ProductFilterUpdateRequestModelValidator()
        {
            RuleFor(x => x.OldProductId)
                .GreaterThan(0).WithMessage("OldProductId must be greater than 0.");

            RuleFor(x => x.OldFilterId)
                .GreaterThan(0).WithMessage("OldFilterId must be greater than 0.");

            RuleFor(x => x.NewProductId)
                .GreaterThan(0).WithMessage("NewProductId must be greater than 0.");

            RuleFor(x => x.NewFilterId)
                .GreaterThan(0).WithMessage("NewFilterId must be greater than 0.");
        }
    }
}
