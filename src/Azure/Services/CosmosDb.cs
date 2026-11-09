using Azure.Models;
using Microsoft.Azure.Cosmos;
using System.IO.Pipelines;

namespace Azure.Services
{
    public class CosmosDb : ICosmosDb
    {
        private readonly CosmosClient _cosmosClient;
        private readonly PartitionKey _cosmosPartitionKey;
        private Database cosmosDatabase;
        private Container cosmosContainer;

        public CosmosDb()
        {
            _cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("CosmosDb:ConnectionString"));
            _cosmosPartitionKey = new PartitionKey(Environment.GetEnvironmentVariable("CosmosDb:PartitionKey"));
        }

        private async Task InitializeContainer()
        {
            cosmosDatabase = _cosmosClient.GetDatabase(Environment.GetEnvironmentVariable("CosmosDb:DatabaseName"));
            await _cosmosClient.CreateDatabaseIfNotExistsAsync(Environment.GetEnvironmentVariable("CosmosDb:DatabaseName"));
            cosmosContainer = cosmosDatabase.GetContainer(Environment.GetEnvironmentVariable("CosmosDb:ContainerName"));
            await cosmosDatabase.CreateContainerIfNotExistsAsync(Environment.GetEnvironmentVariable("CosmosDb:ContainerName"), _cosmosPartitionKey.ToString());

        }
        public async Task AddChunk(Chunk chunk)   
        {
            await InitializeContainer();
            await cosmosContainer.CreateItemAsync(chunk, _cosmosPartitionKey);
        }

        public async Task<List<Chunk>> GetChunksByEmbedding(float[] embedding)
        {
            await InitializeContainer();
            //semantic search query to retrieve chunks based on embedding
            var query = new QueryDefinition("SELECT TOP 10\r\n  *\r\nFROM\r\n  container c\r\nORDER BY RANK\r\n  FULLTEXTSCORE(c.embedding, @embedding)")
                .WithParameter("@embedding", embedding);

            var iterator = cosmosContainer.GetItemQueryIterator<Chunk>(query);

            return await iterator.ReadNextAsync().ContinueWith(task => task.Result.ToList());

        }

    }
}
