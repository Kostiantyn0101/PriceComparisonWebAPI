using OpenAI;


namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddOpenAIExtensions
    {

        public static void AddOpenAI(this WebApplicationBuilder builder)
        {
            var openAiApiKey = builder.Configuration["OpenAI:ApiKey"];
            builder.Services.AddSingleton(new OpenAIClient(openAiApiKey));
        }
    }
}
