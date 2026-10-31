using Azure.Models;
using Azure.Services;

namespace Infrastructure.Services
{
    public class RouteDb
    {
        private readonly string? _configuredDb;
        private readonly CosmosDb _db;

        public RouteDb()
        {
            _configuredDb = Environment.GetEnvironmentVariable("ConfiguredDb");
            _db = _configuredDb?.ToLowerInvariant() switch
            {
                "cosmosdb" => new CosmosDb(),
                // "postgresql" => new PostgreSql(),
                _ => throw new InvalidOperationException("Invalid configured database.")
            };
        }
        public void RouteIngestionToConfiguredDb(Chunk chunk)
        {
            _db.AddChunk(chunk);
        }
    }
}
