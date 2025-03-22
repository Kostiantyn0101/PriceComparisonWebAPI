using PriceComparisonWebAPI.Infrastructure.DependencyInjection;

namespace PriceComparisonWebAPI.Infrastructure
{
    public class ConfigurationService
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.AddConfiguration();
            builder.AddFluentValidation();
            builder.AddDbContext();
            builder.AddIdentity();
            builder.AddRepositories();
            builder.AddServices();
            builder.AddAuth();
            builder.AddSwagger();
            builder.AddAutoMapper();
            builder.AddOthers();
            builder.AddAIServices();
            builder.AddCors();
        }
    }
}
