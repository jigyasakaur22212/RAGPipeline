namespace LLM.Services
{
    public interface ILLMGeneration
    {
        Task<string> GenerateAnswerAsync(string userPrompt, IReadOnlyList<string> chunks);
    }
}
