using AutoMapper;
using Domain.Models.DBModels;
using Domain.Models.Response.Feedback;

namespace PriceComparisonWebAPI.Infrastructure.MapperResolvers
{
    public class FeedbackImageUrlsResolver : IValueResolver<FeedbackDBModel, FeedbackResponseModel, List<string>>
    {
        private readonly IConfiguration _configuration;

        public FeedbackImageUrlsResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<string> Resolve(FeedbackDBModel source, FeedbackResponseModel destination, List<string> destMember, ResolutionContext context)
        {
            if (source.FeedbackImages == null || !source.FeedbackImages.Any())
                return new List<string>();

            var baseUrl = _configuration["FileStorage:ServerURL"]?.TrimEnd('/');
            return source.FeedbackImages
                .Select(fi =>
                    !string.IsNullOrEmpty(fi.ImageUrl)
                    ? $"{baseUrl}/{fi.ImageUrl.TrimStart('/')}"
                    : null)
                .Where(url => url != null)
                .Select(url => url!)
                .ToList();
        }
    }

}
