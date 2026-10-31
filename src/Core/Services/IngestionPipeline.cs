using Infrastructure.Services;
using Azure.Models;
using LLM.Services;

namespace Core.Services
{
    public class IngestionPipeline
    {
        private readonly ChunkingData _chunking;
        private readonly Embedding _embedding;
        private readonly RouteDb _db;

        public IngestionPipeline(Embedding embedding , RouteDb db)
        {
            _chunking = new ChunkingData();
            _embedding = embedding;
            _db = db;
        }
        public async Task IngestData(string data)
        {
            var chunks = _chunking.ChunkData(data);

            foreach (var chunk in chunks)
            {
                var embedding = await _embedding.GenerateEmbeddingAsync(chunk);
                _db.RouteIngestionToConfiguredDb(new Chunk
                {
                    ChunkId = chunks.IndexOf(chunk).ToString(),
                    ActualText = chunk,
                    Embedding = new[] { embedding },
                    CreatedOn = DateTime.UtcNow
                });
            }
        }
    }
}
