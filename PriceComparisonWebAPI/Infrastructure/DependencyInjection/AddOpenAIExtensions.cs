using BLL.Services.AIServices;
using OpenAI;


namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddOpenAIExtensions
    {

        public static void AddAIServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient<AnthropicService>();

            builder.Services.AddSingleton<IAICompletionService, OpenAIService>();
            builder.Services.AddSingleton<IAICompletionService, AnthropicService>();

            builder.Services.AddSingleton<IAIServiceFactory, AIServiceFactory>();

            builder.Services.AddScoped<IProductComparisonService, ProductComparisonService>();

        }
    }
}
