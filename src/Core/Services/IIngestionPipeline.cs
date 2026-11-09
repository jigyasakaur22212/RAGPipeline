namespace Core.Services
{
    public interface IIngestionPipeline
    {
        Task IngestData(string data);

    }
}