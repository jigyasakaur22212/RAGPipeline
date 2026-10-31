using Azure.Models;
using Microsoft.Azure.Cosmos;

namespace Azure.Services
{
    public class CosmosDb
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Container _cosmosContainer;
        private readonly PartitionKey _cosmosPartitionKey;

        public CosmosDb()
        {
            _cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("CosmosDb:ConnectionString"));
            _cosmosContainer = _cosmosClient.GetContainer(Environment.GetEnvironmentVariable("CosmosDb:DatabaseName"), Environment.GetEnvironmentVariable("CosmosDb:ContainerName"));
            _cosmosPartitionKey = new PartitionKey(Environment.GetEnvironmentVariable("CosmosDb:PartitionKey"));
        }

        public void AddChunk(Chunk chunk)   
        {
            _cosmosContainer.CreateItemAsync(chunk, _cosmosPartitionKey);
        }

    }
}
