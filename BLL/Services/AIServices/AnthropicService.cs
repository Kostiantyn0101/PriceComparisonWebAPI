using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace BLL.Services.AIServices
{
    public class AnthropicService : IAICompletionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _modelName;

        public string ProviderName => "Claude";

        public AnthropicService(IConfiguration configuration, HttpClient httpClient)
        {
            _apiKey = configuration["Anthropic:ApiKey"];
            _modelName = configuration["Anthropic:ModelName"] ?? "claude-3-7-sonnet-20250219";

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.anthropic.com/v1/");
        }

        public async Task<string> GetCompletionAsync(string prompt, string systemMessage)
        {
            var request = new
            {
                model = _modelName,
                system = systemMessage,
                messages = new[]
                {
                    //new { role = "system", content = systemMessage },
                    new { role = "user", content = prompt }
                },
                temperature = 0.3,
                max_tokens = 9086
            };
            
            var json = JsonSerializer.Serialize(request,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.PostAsync("messages", content);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<AnthropicResponse>(
                responseString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return responseObject?.Content?.FirstOrDefault()?.Text;
        }

        public class AnthropicResponse
        {
            public string Id { get; set; }
            public string Model { get; set; }
            public ContentBlock[] Content { get; set; }
        }

        public class ContentBlock
        {
            public string Type { get; set; }
            public string Text { get; set; }
        }
    }
}
