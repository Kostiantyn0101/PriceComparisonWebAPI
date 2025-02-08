using AutoMapper;
using Domain.Models.DBModels;
using Domain.Models.Request.Categories;
using Domain.Models.Response.Categories;

namespace PriceComparisonWebAPI.Infrastructure.MapperResolvers
{
    public class CategoryImageUrlResolver : IValueResolver<CategoryDBModel, CategoryResponseModel, string?>
    {
        private readonly IConfiguration _configuration;

        public CategoryImageUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string? Resolve(CategoryDBModel source, CategoryResponseModel destination, string? destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.ImageUrl))
                return null;

            var baseUrl = _configuration["FileStorage:ServerURL"];
            return baseUrl != null ? $"{baseUrl.TrimEnd('/')}/{source.ImageUrl.TrimStart('/')}" : null;
        }
    }
}
