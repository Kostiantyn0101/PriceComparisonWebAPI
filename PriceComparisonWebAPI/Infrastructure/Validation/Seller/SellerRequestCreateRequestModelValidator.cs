using Domain.Models.Request.Seller;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Seller
{
    public class SellerRequestCreateRequestModelValidator : AbstractValidator<SellerRequestCreateRequestModel>
    {
        public SellerRequestCreateRequestModelValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("UserId is required and must be greater than 0");

            RuleFor(x => x.StoreName)
                .NotEmpty().WithMessage("Store name is required")
                .MaximumLength(255).WithMessage("Store name cannot exceed 255 characters");

            RuleFor(x => x.WebsiteUrl)
                .NotEmpty().WithMessage("Website URL is required")
                .MaximumLength(2083).WithMessage("Website URL cannot exceed 2083 characters")
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("Website URL must be a valid URL");

            RuleFor(x => x.ContactPerson)
                .NotEmpty().WithMessage("Contact person name is required")
                .MaximumLength(100).WithMessage("Contact person name cannot exceed 100 characters");

            RuleFor(x => x.ContactPhone)
                .NotEmpty().WithMessage("Contact phone is required")
                .MaximumLength(20).WithMessage("Contact phone cannot exceed 20 characters")
                .Matches(@"^\+?[0-9\s\-\(\)]+$").WithMessage("Contact phone must be a valid phone number");

            RuleFor(x => x.StoreComment)
                .MaximumLength(1000).WithMessage("Store comment cannot exceed 1000 characters")
                .When(x => x.StoreComment != null);
        }
    }
}
