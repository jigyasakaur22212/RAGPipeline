using Azure.Models;

namespace Infrastructure.Services
{
    public interface IRouteDb
    {
        void RouteIngestionToConfiguredDb(Chunk chunk);

    }
}