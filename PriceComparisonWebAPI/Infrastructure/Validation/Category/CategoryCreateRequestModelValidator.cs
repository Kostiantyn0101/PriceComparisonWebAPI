using Domain.Models.Request.Categories;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Category
{
    public class CategoryCreateRequestModelValidator : AbstractValidator<CategoryCreateRequestModel>
    {
        public CategoryCreateRequestModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Category name required.")
                .MaximumLength(100).WithMessage("Category name length must be less than 100 characters");

            RuleFor(x => x.ParentCategoryId)
                .GreaterThanOrEqualTo(1).When(x => x.ParentCategoryId.HasValue)
                .WithMessage("Parent category id must be 1 or greater");

            RuleFor(x => x.Image)
            .ImageFileRules();

            RuleFor(x => x.Icon)
                .ImageFileRules();

            RuleFor(x => x.DisplayOrder)
                .GreaterThan(0).WithMessage("Display order must be greater than 0");
        }
    }

}
