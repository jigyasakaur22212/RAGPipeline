using Microsoft.Extensions.AI;
using OpenAI.Embeddings;

namespace LLM.Services
{
    public class Embedding : IEmbedding
    {
        private readonly IEmbeddingGenerator<string, Embedding<float>> _generator;

        public Embedding()
        {
            _generator = new EmbeddingClient(
                Environment.GetEnvironmentVariable("OpenAI:EmbeddingModelName") ?? Environment.GetEnvironmentVariable("OpenApi:EmbeddingModelName"),
                Environment.GetEnvironmentVariable("OpenAI:ApiKey") ?? Environment.GetEnvironmentVariable("OpenApi:ApiKey"))
                .AsIEmbeddingGenerator();
        }

        public async Task<float[]> GenerateEmbeddingAsync(string text)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(text);

            var embedding = await _generator.GenerateAsync(text);
            return embedding.Vector.ToArray();
        }
    }
}
