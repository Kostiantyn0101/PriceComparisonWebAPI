using Domain.Models.Request.Seller;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Seller
{
    public class SellerUpdateRequestModelValidator : AbstractValidator<SellerUpdateRequestModel>
    {
        public SellerUpdateRequestModelValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Seller Id must be greater than 0.");

            RuleFor(x => x.ApiKey)
                .NotEmpty().WithMessage("ApiKey is required.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");
        }
    }
}
