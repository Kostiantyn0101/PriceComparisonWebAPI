using System.Text.Json.Serialization;
using PriceComparisonWebAPI.Filters;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddOthersExtension
    {
        public static void AddOthers(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddControllers(options =>
                {
                    options.Filters.Add<GlobalExceptionFilter>();
                })
               .AddJsonOptions(options =>
                {
                    //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

                    options.JsonSerializerOptions.WriteIndented = true;
                });
        }

    }
}
