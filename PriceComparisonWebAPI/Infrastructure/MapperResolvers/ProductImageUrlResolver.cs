using AutoMapper;
using Domain.Models.DBModels;
using Domain.Models.Response.Products;

namespace PriceComparisonWebAPI.Infrastructure.MapperResolvers
{
    public class ProductImageUrlResolver : IValueResolver<ProductImageDBModel, ProductImageResponseModel, string?>
    {
        private readonly IConfiguration _configuration;

        public ProductImageUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string? Resolve(ProductImageDBModel source, ProductImageResponseModel destination, string? destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.ImageUrl))
                return null;

            var baseUrl = _configuration["FileStorage:ServerURL"];
            return baseUrl != null ? $"{baseUrl.TrimEnd('/')}/{source.ImageUrl.TrimStart('/')}" : null;
        }
    }
}
