using Azure.Models;

namespace Azure.Services
{
    public interface ICosmosDb
    {
        Task AddChunk(Chunk chunk);
        Task<List<Chunk>> GetChunksByEmbedding(float[] embedding);

    }
}