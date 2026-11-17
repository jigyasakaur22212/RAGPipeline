using Azure.Models;
using Infrastructure.Services;
using LLM.Services;

namespace Core.Services
{
    public class IngestionPipeline : IIngestionPipeline
    {
        private readonly IChunkingData _chunking;
        private readonly IEmbedding _embedding;
        private readonly IRouteDb _db;

        public IngestionPipeline(IChunkingData chunking, IEmbedding embedding, IRouteDb db)
        {
            _chunking = chunking;
            _embedding = embedding;
            _db = db;
        }

        public async Task IngestData(string data)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(data);

            var chunks = _chunking.ChunkData(data);
            for (var index = 0; index < chunks.Count; index++)
            {
                var chunk = chunks[index];
                var embedding = await _embedding.GenerateEmbeddingAsync(chunk);

                await _db.RouteIngestionToConfiguredDbAsync(new Chunk
                {
                    ChunkId = index.ToString(),
                    ActualText = chunk,
                    Embedding = embedding,
                    CreatedOn = DateTime.UtcNow
                });
            }
        }
    }
}
