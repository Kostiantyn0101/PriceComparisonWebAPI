namespace BLL.Services.AIServices
{
    public enum AIProvider
    {
        OpenAI,
        Claude
    }
    public interface IAIServiceFactory
    {
        IAICompletionService GetService(AIProvider provider);
        IAICompletionService GetDefaultService();
    }
}
