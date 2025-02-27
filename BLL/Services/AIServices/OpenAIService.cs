using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;

namespace BLL.Services.AIServices
{
    public class OpenAIService : IAICompletionService
    {
        private readonly OpenAIClient _client;
        private readonly string _modelName;

        public string ProviderName => "OpenAI";

        public OpenAIService(IConfiguration configuration)
        {
            var apiKey = configuration["OpenAI:ApiKey"];
            _modelName = configuration["OpenAI:ModelName"] ?? "chatgpt-4o-mini";

            _client = new OpenAIClient(apiKey);
        }

        public async Task<string> GetCompletionAsync(string prompt, string systemMessage)
        {
            //create request to OpenAI
            var chatRequest = new ChatRequest(
                new[]
                {
                    new Message(Role.System, systemMessage),
                    new Message(Role.User, prompt)
                },
                model: _modelName,
                temperature: 0.3f
            );

            //request to OpenAI
            var response = await _client.ChatEndpoint.GetCompletionAsync(chatRequest);
            return response.FirstChoice.Message.Content.ToString();
        }
    }
}
