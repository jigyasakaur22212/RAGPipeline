
namespace Core.Services
{
    public interface IRetrivalPipeline
    {
        Task<string> GenerateAnswerAsync(string prompt);

    }
}
