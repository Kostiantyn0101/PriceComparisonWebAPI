using AutoMapper;
using BLL.Services.ProductService;
using Domain.Models.Response.Gpt;
using Domain.Models.Response.Gpt.Product;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Threads;
using System.Text;
using System.Text.Json;
using Role = OpenAI.Role;


namespace BLL.Services.AIServices
{
    public class ProductComparisonService : IProductComparisonService
    {
        private readonly IAIServiceFactory _aiServiceFactory;
        private readonly IProductCharacteristicService _productCharacteristicService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductComparisonService(
            IAIServiceFactory aiServiceFactory,
            IProductCharacteristicService productCharacteristicService,
            IProductService productService,
            IMapper mapper)
        {
            _aiServiceFactory = aiServiceFactory;
            _productCharacteristicService = productCharacteristicService;
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<AIComparisonProductCharacteristicResponseModel> CompareProductsAsync(int productIdA, int productIdB, AIProvider? provider = null)
        {
            // get character for product
            var productAData = await _productCharacteristicService.GetDetailedCharacteristics(productIdA);
            var productBData = await _productCharacteristicService.GetDetailedCharacteristics(productIdB);

            var productAList = await _productService.GetFromConditionAsync(t => t.Id == productIdA);
            var productBList = await _productService.GetFromConditionAsync(t => t.Id == productIdB);

            var productATitle = productAList.FirstOrDefault()?.Title ?? "Product A";
            var productBTitle = productBList.FirstOrDefault()?.Title ?? "Product B";

            var simplifiedProductA = _mapper.Map<List<SimplifiedProductCharacteristicGroupResponseModel>>(productAData);
            var simplifiedProductB = _mapper.Map<List<SimplifiedProductCharacteristicGroupResponseModel>>(productBData);

            // prompt to GPT return JSON
            string prompt = GeneratePrompt(simplifiedProductA, simplifiedProductB, productATitle, productBTitle);

            // service provider
            var aiService = provider.HasValue ? _aiServiceFactory.GetService(provider.Value) : _aiServiceFactory.GetDefaultService();

            // send request
            string rawResponse = await aiService.GetCompletionAsync(prompt, "You are an expert product analyst.");

            // process response
            var result = ProcessAIResponse(rawResponse, simplifiedProductA, simplifiedProductB, productATitle, productBTitle);
            result.AIProvider = aiService.ProviderName;

            return result;
        }

        private string GeneratePrompt(
            IEnumerable<SimplifiedProductCharacteristicGroupResponseModel> productA,
            IEnumerable<SimplifiedProductCharacteristicGroupResponseModel> productB,
            string productAName,
            string productBName)
        {
            var sb = new StringBuilder();

            // Basic instructions with clearer structure
            sb.AppendLine("# Product Comparison Analysis");
            sb.AppendLine("You are an expert in product comparison tasked with analyzing two products based on their characteristics and determining which has better features.");
            sb.AppendLine();

            // Instructions section with numbered list for clarity
            sb.AppendLine("## Instructions");
            sb.AppendLine($"1. Compare the characteristics of \"{productAName}\" and \"{productBName}\" group by group.");
            //sb.AppendLine("2. Set 'isHighlighted: true' for the product where this characteristic is better.");
            sb.AppendLine("2. Встанови `isHighlighted: true` для того продукту в якого дана характеристика краща.");
            sb.AppendLine("3. Leave both products' `isHighlighted: false` when characteristics are equal");
            //sb.AppendLine("4. Не пропускай жодних характеристик, вивести потрібно всі для обох товарів.");
            sb.AppendLine("4. Do not skip any characteristics; all of them must be displayed for both products.");
            sb.AppendLine();

            // Input validation warning
            if (!productA.Any() || !productB.Any())
            {
                //sb.AppendLine("⚠️ WARNING: At least one product appears to have no characteristics. Please verify input data.");
            }

            // Expected output format with more detailed example
            sb.AppendLine("## Return Format");
            sb.AppendLine("Return ONLY valid JSON following this structure:");
            sb.AppendLine(@"
                        {
                          ""productA"": [
                            {
                              ""characteristicGroupTitle"": ""Performance"",
                              ""productCharacteristics"": [
                                {
                                  ""characteristicTitle"": ""CPU"",
                                  ""value"": ""Intel i7-1165G7"",
                                  ""isHighlighted"": true
                                }
                              ]
                            }
                          ],
                          ""productB"": [
                            {
                              ""characteristicGroupTitle"": ""Performance"",
                              ""productCharacteristics"": [
                                {
                                  ""characteristicTitle"": ""CPU"",
                                  ""value"": ""Intel i5-1135G7"",
                                  ""isHighlighted"": false
                                }
                              ]
                            }
                          ],
                      ""explanation"": ""Product A offers superior performance with its i7 processor compared to Product B's i5, while both products have similar build quality and features.""
                    }");
            sb.AppendLine();

            // Product details with validation
            sb.AppendLine("## " + productAName + " Details");
            try
            {
                sb.AppendLine(JsonSerializer.Serialize(productA,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    }));
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Error serializing Product A: {ex.Message}");
            }

            sb.AppendLine();
            sb.AppendLine("## " + productBName + " Details");
            try
            {
                sb.AppendLine(JsonSerializer.Serialize(productB,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    }));
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Error serializing Product B: {ex.Message}");
            }

            sb.AppendLine();
            sb.AppendLine("Important: Return ONLY valid JSON without any commentary, explanations, or markdown formatting.");

            return sb.ToString();
        }

        

        private string CleanGptResponse(string input)
        {
            string result = input.Trim();
            if (result.StartsWith("```json"))
            {
                result = result.Substring(7);
            }
            if (result.EndsWith("```"))
            {
                result = result.Substring(0, result.Length - 3);
            }
            return result;
        }

        private bool IsValidJson(string input)
        {
            input = input.Trim();
            if (string.IsNullOrEmpty(input) || !input.StartsWith("{") && !input.StartsWith("["))
            {
                return false;
            }

            try
            {
                using (JsonDocument.Parse(input)) { return true; }
            }
            catch (JsonException)
            {
                return false;
            }
        }

        private AIComparisonProductCharacteristicResponseModel ProcessAIResponse(string gptResponse,
                    IEnumerable<SimplifiedProductCharacteristicGroupResponseModel> defaultA,
                    IEnumerable<SimplifiedProductCharacteristicGroupResponseModel> defaultB,
                    string productATitle,
                    string productBTitle)
        {
            string cleanedResponse = CleanGptResponse(gptResponse);

            if (!IsValidJson(cleanedResponse))
            {
                return new AIComparisonProductCharacteristicResponseModel
                {
                    ProductATitle = productATitle,
                    ProductBTitle = productBTitle,
                    ProductA = defaultA,
                    ProductB = defaultB,
                    Explanation = "GPT response is not valid JSON"
                };
            }

            try
            {
                var parsed = JsonSerializer.Deserialize<AIComparisonProductCharacteristicResponseModel>(
                    cleanedResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (parsed != null)
                {
                    if (string.IsNullOrWhiteSpace(parsed.ProductATitle))
                    {
                        parsed.ProductATitle = productATitle;
                    }
                    if (string.IsNullOrWhiteSpace(parsed.ProductBTitle))
                    {
                        parsed.ProductBTitle = productBTitle;
                    }

                    if (parsed.ProductA == null)
                    {
                        parsed.ProductA = defaultA;
                    }
                    if (parsed.ProductB == null)
                    {
                        parsed.ProductB = defaultB;
                    }
                    if (string.IsNullOrWhiteSpace(parsed.Explanation))
                    {
                        parsed.Explanation = "No explanation provided";
                    }
                    return parsed;
                }
            }
            catch (JsonException)
            {
                return new AIComparisonProductCharacteristicResponseModel
                {
                    ProductATitle = productATitle,
                    ProductBTitle = productBTitle,
                    ProductA = defaultA,
                    ProductB = defaultB,
                    Explanation = "GPT returned invalid JSON"
                };
            }

            return new AIComparisonProductCharacteristicResponseModel
            {
                ProductATitle = productATitle,
                ProductBTitle = productBTitle,
                ProductA = defaultA,
                ProductB = defaultB,
                Explanation = "Unexpected error during GPT processing"
            };
        }
    }
}