using Domain.Models.Request.Categories;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation
{
    public class RelatedCategoryRequestModelValidator : AbstractValidator<RelatedCategoryRequestModel>
    {
        public RelatedCategoryRequestModelValidator()
        {
            RuleFor(x => x.CategoryId)
                 .GreaterThan(0).WithMessage("Category id must be greater than 0");

            RuleFor(x => x.RelatedCategoryId)
                .GreaterThan(0).WithMessage("Related category id must be greater than 0");

            RuleFor(x => x)
                .Must(x => x.CategoryId != x.RelatedCategoryId)
                .WithMessage("Category id and related category id must be different");
        }
    }
}
