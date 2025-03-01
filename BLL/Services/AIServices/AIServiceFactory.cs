using Microsoft.Extensions.Configuration;

namespace BLL.Services.AIServices
{
    public class AIServiceFactory : IAIServiceFactory
    {
        private readonly IEnumerable<IAICompletionService> _services;
        private readonly IConfiguration _configuration;

        public AIServiceFactory(IEnumerable<IAICompletionService> services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        public IAICompletionService GetService(AIProvider provider)
        {
            return provider switch
            {
                AIProvider.OpenAI => _services.FirstOrDefault(s => s.ProviderName == "OpenAI"),
                AIProvider.Claude => _services.FirstOrDefault(s => s.ProviderName == "Claude"),
                _ => throw new ArgumentOutOfRangeException(nameof(provider), $"Provider {provider} is not supported")
            };
        }
        public IAICompletionService GetDefaultService()
        {
            string defaultProvider = _configuration["AI:DefaultProvider"] ?? "OpenAI";

            if (Enum.TryParse<AIProvider>(defaultProvider, out var provider))
            {
                return GetService(provider);
            }

            return _services.FirstOrDefault() ??
                throw new InvalidOperationException("No AI service providers are registered");
        }
    }
}
