using Domain.Models.Request.Seller;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Seller
{
    public class SellerCreateRequestModelValidator : AbstractValidator<SellerCreateRequestModel>
    {
        public SellerCreateRequestModelValidator()
        {
            RuleFor(x => x.StoreName)
                .NotEmpty().WithMessage("Store name is required.");
            
            RuleFor(x => x.WebsiteUrl)
            .NotEmpty().WithMessage("Website url is required.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");
        }
    }
}
