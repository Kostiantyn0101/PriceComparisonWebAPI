namespace BLL.Services.AIServices
{
    public interface IAICompletionService
    {
        Task<string> GetCompletionAsync(string prompt, string systemMessage);
        string ProviderName { get; }
    }
}
