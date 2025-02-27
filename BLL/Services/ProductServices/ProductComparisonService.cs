using AutoMapper;
using BLL.Services.ProductService;
using Domain.Models.Response.Gpt;
using Domain.Models.Response.Gpt.Product;
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
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductComparisonService(
            OpenAIClient openAIClient,
            IProductCharacteristicService productCharacteristicService,
            IProductService productService,
            IMapper mapper)
        {
            _client = openAIClient;
            _productCharacteristicService = productCharacteristicService;
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<GptComparisonProductCharacteristicResponseModel> CompareProductsAsync(int productIdA, int productIdB)
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

            // send request
            string rawGptResponse = await SendChatRequestAsync(prompt, "You are an expert product analyst.");

            // process response
            return ProcessGptResponse(rawGptResponse, simplifiedProductA, simplifiedProductB, productATitle, productBTitle);
        }

        private async Task<string> SendChatRequestAsync(string prompt, string systemMessage)
        {
            //create request to OpenAI
            var chatRequest = new ChatRequest(
                new[]
                {
                    new Message(Role.System, systemMessage),
                    new Message(Role.User, prompt)
                },
                model: "gpt-4o-mini",
                temperature: 0.3f
            );

            //request to OpenAI
            var response = await _client.ChatEndpoint.GetCompletionAsync(chatRequest);
            return response.FirstChoice.Message.Content.ToString();
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

        //private string GeneratePrompt(
        //                    IEnumerable<SimplifiedProductCharacteristicGroupRsponseModel> productA,
        //                    IEnumerable<SimplifiedProductCharacteristicGroupRsponseModel> productB)
        //{
        //    var sb = new StringBuilder();
        //    sb.AppendLine("You are an expert in product comparison. Your task is to analyze two products based on their characteristics and determine which product has better features.");
        //    sb.AppendLine();
        //    sb.AppendLine("### **Instructions:**");
        //    sb.AppendLine("1. You will receive two products in JSON format, where each product has a list of characteristic groups.");
        //    sb.AppendLine("2. Compare the characteristics of Product A and Product B group by group.");
        //    sb.AppendLine("3. Identify the **better characteristic** and set `isHighlighted: true` for the product that has the advantage.");
        //    sb.AppendLine("4. If both characteristics are equal, do not highlight any characteristic.");
        //    sb.AppendLine("5. Provide a brief **explanation** for the comparison.");
        //    sb.AppendLine();
        //    sb.AppendLine("### **Comparison Rules:**");
        //    sb.AppendLine("- **Higher numerical values** (e.g., processor speed, RAM, battery capacity) are generally better.");
        //    sb.AppendLine("- **Boolean values** (`true` is better than `false` if it represents a feature like waterproofing, extra security, or advanced functionality).");
        //    sb.AppendLine("- **Text values** (e.g., material type, additional features) should be compared based on **quality, durability, and consumer preferences**.");
        //    sb.AppendLine("- If the characteristic **exists in one product but not the other**, highlight the one that has it.");
        //    sb.AppendLine("- If both products have **identical values**, do not highlight either.");
        //    sb.AppendLine();
        //    sb.AppendLine("### **Return Format:**");
        //    sb.AppendLine("Ensure the response is a valid JSON object with the following structure:");
        //    sb.AppendLine(@"
        //                    {
        //                      ""productA"": [
        //                        {
        //                          ""characteristicGroupTitle"": ""string"",
        //                          ""productCharacteristics"": [
        //                            {
        //                              ""characteristicTitle"": ""string"",
        //                              ""characteristicDataType"": ""string"",
        //                              ""valueText"": ""string"",
        //                              ""valueNumber"": 0,
        //                              ""valueBoolean"": true,
        //                              ""valueDate"": ""2025-02-25T13:54:36.128Z"",
        //                              ""isHighlighted"": true
        //                            }
        //                          ]
        //                        }
        //                      ],
        //                      ""productB"": [
        //                        {
        //                          ""characteristicGroupTitle"": ""string"",
        //                          ""productCharacteristics"": [
        //                            {
        //                              ""characteristicTitle"": ""string"",
        //                              ""characteristicDataType"": ""string"",
        //                              ""valueText"": ""string"",
        //                              ""valueNumber"": 0,
        //                              ""valueBoolean"": true,
        //                              ""valueDate"": ""2025-02-25T13:54:36.128Z"",
        //                              ""isHighlighted"": false
        //                            }
        //                          ]
        //                        }
        //                      ],
        //                      ""explanation"": ""String for explantation""
        //                    }");
        //    sb.AppendLine();
        //    sb.AppendLine("### **Product A Details:**");
        //    sb.AppendLine(JsonSerializer.Serialize(productA,
        //        new JsonSerializerOptions
        //        {
        //            WriteIndented = true,
        //            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        //        }));
        //    sb.AppendLine();
        //    sb.AppendLine("### **Product B Details:**");
        //    sb.AppendLine(JsonSerializer.Serialize(productB,
        //        new JsonSerializerOptions
        //        {
        //            WriteIndented = true,
        //            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        //        }));
        //    sb.AppendLine();
        //    sb.AppendLine("Return only valid JSON without any extra commentary.");

        //    return sb.ToString();
        //}

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
            if (string.IsNullOrEmpty(input) || (!input.StartsWith("{") && !input.StartsWith("[")))
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

        private GptComparisonProductCharacteristicResponseModel ProcessGptResponse(string gptResponse,
                    IEnumerable<SimplifiedProductCharacteristicGroupResponseModel> defaultA,
                    IEnumerable<SimplifiedProductCharacteristicGroupResponseModel> defaultB,
                    string productATitle,
                    string productBTitle)
        {
            string cleanedResponse = CleanGptResponse(gptResponse);

            if (!IsValidJson(cleanedResponse))
            {
                return new GptComparisonProductCharacteristicResponseModel
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
                var parsed = JsonSerializer.Deserialize<GptComparisonProductCharacteristicResponseModel>(
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
                return new GptComparisonProductCharacteristicResponseModel
                {
                    ProductATitle = productATitle,
                    ProductBTitle = productBTitle,
                    ProductA = defaultA,
                    ProductB = defaultB,
                    Explanation = "GPT returned invalid JSON"
                };
            }

            return new GptComparisonProductCharacteristicResponseModel
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