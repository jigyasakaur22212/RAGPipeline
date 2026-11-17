using Azure.Models;
using Azure.Services;

namespace Infrastructure.Services
{
    public class RouteDb : IRouteDb
    {
        private readonly ICosmosDb _db;

        public RouteDb(ICosmosDb db)
        {
            _db = db;
        }

        public Task RouteIngestionToConfiguredDbAsync(Chunk chunk)
        {
            return _db.AddChunk(chunk);
        }
    }
}
