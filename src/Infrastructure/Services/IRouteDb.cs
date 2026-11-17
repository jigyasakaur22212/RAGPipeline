using Azure.Models;

namespace Infrastructure.Services
{
    public interface IRouteDb
    {
        Task RouteIngestionToConfiguredDbAsync(Chunk chunk);
    }
}
