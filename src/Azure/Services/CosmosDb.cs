using Azure.Models;
using Microsoft.Azure.Cosmos;
using System.Collections.ObjectModel;

namespace Azure.Services
{
    public class CosmosDb : ICosmosDb
    {
        private readonly CosmosClient _cosmosClient;
        private readonly string _databaseName;
        private readonly string _containerName;
        private readonly string _partitionKeyPath;
        private readonly int _embeddingDimensions;
        private readonly SemaphoreSlim _initializationLock = new(1, 1);
        private Container? _cosmosContainer;

        public CosmosDb()
        {
            var connectionString = GetRequiredSetting("CosmosDb:ConnectionString");
            _databaseName = GetRequiredSetting("CosmosDb:DatabaseName");
            _containerName = GetRequiredSetting("CosmosDb:ContainerName");
            _partitionKeyPath = Environment.GetEnvironmentVariable("CosmosDb:PartitionKey") ?? "/ChunkId";
            _embeddingDimensions = int.TryParse(Environment.GetEnvironmentVariable("CosmosDb:EmbeddingDimensions"), out var dimensions)
                ? dimensions
                : 1536;
            _cosmosClient = new CosmosClient(connectionString);
        }

        public async Task AddChunk(Chunk chunk)
        {
            var container = await GetContainerAsync();
            await container.UpsertItemAsync(chunk, new PartitionKey(chunk.ChunkId));
        }

        public async Task<List<Chunk>> GetChunksByEmbedding(float[] embedding)
        {
            ArgumentNullException.ThrowIfNull(embedding);

            var container = await GetContainerAsync();
            var query = new QueryDefinition(
                "SELECT TOP 10 * FROM c ORDER BY VectorDistance(c.Embedding, @embedding)")
                .WithParameter("@embedding", embedding);
            var results = new List<Chunk>();
            using var iterator = container.GetItemQueryIterator<Chunk>(query);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        private async Task<Container> GetContainerAsync()
        {
            if (_cosmosContainer is not null)
            {
                return _cosmosContainer;
            }

            await _initializationLock.WaitAsync();
            try
            {
                if (_cosmosContainer is not null)
                {
                    return _cosmosContainer;
                }

                var databaseResponse = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseName);
                var embedding = new Embedding
                {
                    Path = "/Embedding",
                    DataType = VectorDataType.Float32,
                    DistanceFunction = DistanceFunction.Cosine,
                    Dimensions = _embeddingDimensions
                };
                var properties = new ContainerProperties(_containerName, _partitionKeyPath)
                {
                    VectorEmbeddingPolicy = new VectorEmbeddingPolicy(new Collection<Embedding> { embedding }),
                    IndexingPolicy = new IndexingPolicy
                    {
                        VectorIndexes =
                        {
                            new VectorIndexPath
                            {
                                Path = "/Embedding",
                                Type = VectorIndexType.QuantizedFlat
                            }
                        }
                    }
                };

                var response = await databaseResponse.Database.CreateContainerIfNotExistsAsync(properties);
                _cosmosContainer = response.Container;
                return _cosmosContainer;
            }
            finally
            {
                _initializationLock.Release();
            }
        }

        private static string GetRequiredSetting(string name)
        {
            return Environment.GetEnvironmentVariable(name)
                ?? throw new InvalidOperationException($"Missing required setting '{name}'.");
        }
    }
}
