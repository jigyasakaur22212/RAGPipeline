using Azure.Models;
using Azure.Services;
using LLM.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    public class RetrivalPipeline : IRetrivalPipeline
    {
        private IEmbedding _embeddingService;
        private ICosmosDb _cosmosDb;

        public RetrivalPipeline(IEmbedding embeddingService, ICosmosDb cosmosDb)
        {
            _embeddingService = embeddingService;
            _cosmosDb = cosmosDb;
        }
        public async Task<List<Chunk>> GetRelevantChunks(string prompt)
        {
            var promptEmbedding = await _embeddingService.GenerateEmbeddingAsync(prompt);

            var relevantChunks = await _cosmosDb.GetChunksByEmbedding(new float[] { promptEmbedding });

            return relevantChunks;  
        }
    }
}
