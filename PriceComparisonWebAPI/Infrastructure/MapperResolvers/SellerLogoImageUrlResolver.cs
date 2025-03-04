using AutoMapper;
using Domain.Models.DBModels;
using Domain.Models.Response.Seller;

namespace PriceComparisonWebAPI.Infrastructure.MapperResolvers
{
    public class SellerLogoImageUrlResolver : IValueResolver<SellerDBModel, SellerResponseModel, string?>
    {
        private readonly IConfiguration _configuration;

        public SellerLogoImageUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string? Resolve(SellerDBModel source, SellerResponseModel destination, string? destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.LogoImageUrl))
                return null;

            var baseUrl = _configuration["FileStorage:ServerURL"];
            return baseUrl != null ? $"{baseUrl.TrimEnd('/')}/{source.LogoImageUrl.TrimStart('/')}" : null;
        }
    }
}
