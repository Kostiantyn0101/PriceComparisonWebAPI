using BLL.Services.ProductService;
using Domain.Models.Response.Gpt;
using Domain.Models.Response.Products;
using OpenAI;
using OpenAI.Chat;
using System.Text;
using System.Text.Json;
using Role = OpenAI.Role;

namespace BLL.Services.ProductServices
{
    public class ProductComparisonService : IProductComparisonService
    {
        private readonly OpenAIClient _client;
        private readonly IProductCharacteristicService _productCharacteristicService;

        public ProductComparisonService(
            OpenAIClient openAIClient,
            IProductCharacteristicService productCharacteristicService)
        {
            _client = openAIClient;
            _productCharacteristicService = productCharacteristicService;
        }

        public async Task<(IEnumerable<ProductCharacteristicGroupResponseModel> ProductA,
                           IEnumerable<ProductCharacteristicGroupResponseModel> ProductB,
                           string Explanation)>
               CompareProductsAsync(int productIdA, int productIdB)
        {
            // 1. get character for product
            var productAData = await _productCharacteristicService.GetDetailedCharacteristics(productIdA);
            var productBData = await _productCharacteristicService.GetDetailedCharacteristics(productIdB);


            // 2. prompt to GPT return JSON
            string prompt = GeneratePrompt(productAData, productBData);

            // 3. create reques OpenAI
            var chatRequest = new ChatRequest(
                new[]
                {
                    new Message(Role.System, "You are an expert product analyst."),
                    new Message(Role.User, prompt)
                },
                model: "gpt-4o-mini",
                temperature: 0.7f
            );

            // 4. reques
            var response = await _client.ChatEndpoint.GetCompletionAsync(chatRequest);

            if (response == null || response.FirstChoice?.Message?.Content == null)
            {
                return (productAData, productBData, "No GPT answer");
            }

            var gptAnswer = response.FirstChoice.Message.Content;


            // 5. pars JSON-response
            try
            {
                var parsed = JsonSerializer.Deserialize<GptComparisonResponse>(
                    gptAnswer,
                    new JsonSerializerOptions 
                    {
                        PropertyNameCaseInsensitive = true 
                    });

                if (parsed != null)
                {
                    var updatedA = parsed.ProductA ?? productAData;
                    var updatedB = parsed.ProductB ?? productBData;
                    var explanation = parsed.Explanation ?? "No explanation provided";
                    return (updatedA, updatedB, explanation);
                }
            }
            catch (JsonException)
            {
                return (productAData, productBData, "GPT returned invalid JSON");
            }

            return (productAData, productBData, "Unexpected error during GPT processing");
        }


        private string GeneratePrompt(
        IEnumerable<ProductCharacteristicGroupResponseModel> productA,
        IEnumerable<ProductCharacteristicGroupResponseModel> productB)
        {
            var sb = new StringBuilder();
            sb.AppendLine("We have two products with the following characteristics.");
            sb.AppendLine("Please compare them and mark with 'IsHighlighted': true for any characteristic that is an advantage for that product.");
            sb.AppendLine("Ensure the response is a valid JSON object and contains only the JSON structure. Do not include any explanations or comments outside of the JSON.");
            sb.AppendLine("Return your answer strictly in JSON with the following structure:");
            sb.AppendLine(@"
                            {
                              ""productA"": [ ... ],
                              ""productB"": [ ... ],
                              ""explanation"": ""...""
                            }
                            ");
            sb.AppendLine("Product A details:");
            sb.AppendLine(JsonSerializer.Serialize(productA, new JsonSerializerOptions { WriteIndented = true }));
            sb.AppendLine("Product B details:");
            sb.AppendLine(JsonSerializer.Serialize(productB, new JsonSerializerOptions { WriteIndented = true }));
            sb.AppendLine("Return only valid JSON without any extra commentary.");
            return sb.ToString();
        }
    }
}