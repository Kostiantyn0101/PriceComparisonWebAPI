using Domain.Models.Request.Seller;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Seller
{
    public class SellerRequestProcessRequestModelValidator : AbstractValidator<SellerRequestProcessRequestModel>
    {
        public SellerRequestProcessRequestModelValidator()
        {
            RuleFor(x => x.Id)
                 .NotEmpty()
                 .GreaterThan(0).WithMessage("Id is required and must be greater than 0");

            RuleFor(x => x.RefusalReason)
                .NotEmpty().When(x => x.IsApproved == false)
                .WithMessage("Refusal reason is required when request is not approved")
                .MaximumLength(1000).WithMessage("Refusal reason cannot exceed 1000 characters");
        }
    }
}
