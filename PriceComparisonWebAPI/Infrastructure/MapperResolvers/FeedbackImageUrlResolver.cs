using AutoMapper;
using Domain.Models.DBModels;
using Domain.Models.Response.Feedback;
using Domain.Models.Response.Products;

namespace PriceComparisonWebAPI.Infrastructure.MapperResolvers
{
    public class FeedbackImageUrlResolver : IValueResolver<FeedbackImageDBModel, FeedbackImageResponseModel, string?>
    {
        private readonly IConfiguration _configuration;

        public FeedbackImageUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string? Resolve(FeedbackImageDBModel source, FeedbackImageResponseModel destination, string? destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.ImageUrl))
                return null;

            var baseUrl = _configuration["FileStorage:ServerURL"];
            return baseUrl != null ? $"{baseUrl.TrimEnd('/')}/{source.ImageUrl.TrimStart('/')}" : null;
        }
    }
}
