using Azure.Models;

namespace Core.Services
{
    public interface IRetrivalPipeline
    {
        Task<List<Chunk>> GetRelevantChunks(string prompt);

    }
}