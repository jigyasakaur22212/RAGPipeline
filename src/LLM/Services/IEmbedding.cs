namespace LLM.Services
{
    public interface IEmbedding
    {
        Task<float> GenerateEmbeddingAsync(string text);
    }
}