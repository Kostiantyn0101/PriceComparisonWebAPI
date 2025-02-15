using Domain.Models.Request.Seller;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation
{
    public class SellerCreateRequestModelValidator : AbstractValidator<SellerCreateRequestModel>
    {
        public SellerCreateRequestModelValidator()
        {
            RuleFor(x => x.ApiKey)
                .NotEmpty().WithMessage("ApiKey is required.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");
        }
    }
}
