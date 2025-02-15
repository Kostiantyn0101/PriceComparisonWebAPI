using Domain.Models.Request.Categories;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Category
{
    public class RelatedCategoryUpdateRequestModelValidator : AbstractValidator<RelatedCategoryUpdateRequestModel>
    {
        public RelatedCategoryUpdateRequestModelValidator()
        {
            RuleFor(x => x.OldCategoryId)
                .GreaterThan(0).WithMessage("Old category id must be greater than 0");

            RuleFor(x => x.OldRelatedCategoryId)
                .GreaterThan(0).WithMessage("Old related category id must be greater than 0");

            RuleFor(x => x)
                .Must(x => x.OldCategoryId != x.OldRelatedCategoryId)
                .WithMessage("Old category id and old related category id must be different");

            RuleFor(x => x.NewCategoryId)
                .GreaterThan(0).WithMessage("New category id must be greater than 0");

            RuleFor(x => x.NewRelatedCategoryId)
                .GreaterThan(0).WithMessage("New related category id must be greater than 0");

            RuleFor(x => x)
                .Must(x => x.NewCategoryId != x.NewRelatedCategoryId)
                .WithMessage("New category id and new related category id must be different");
        }
    }
}
