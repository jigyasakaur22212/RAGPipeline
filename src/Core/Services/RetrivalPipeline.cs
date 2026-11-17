using Azure.Services;
using LLM.Services;

namespace Core.Services
{
    public class RetrivalPipeline : IRetrivalPipeline
    {
        private readonly IEmbedding _embeddingService;
        private readonly ICosmosDb _cosmosDb;
        private readonly ILLMGeneration _llmGeneration;

        public RetrivalPipeline(
            IEmbedding embeddingService,
            ICosmosDb cosmosDb,
            ILLMGeneration llmGeneration)
        {
            _embeddingService = embeddingService;
            _cosmosDb = cosmosDb;
            _llmGeneration = llmGeneration;
        }

        public async Task<string> GenerateAnswerAsync(string prompt)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(prompt);

            var promptEmbedding = await _embeddingService.GenerateEmbeddingAsync(prompt);
            var relevantChunks = await _cosmosDb.GetChunksByEmbedding(promptEmbedding);
            var context = relevantChunks.Select(chunk => chunk.ActualText).ToList();

            return await _llmGeneration.GenerateAnswerAsync(prompt, context);
        }
    }
}
