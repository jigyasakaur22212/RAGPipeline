using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace RAGPipeline.Controllers
{
    public class RetrivalGeneration : Controller
    {
        private IRetrivalPipeline _retrivalPipeline;

        public RetrivalGeneration(IRetrivalPipeline retrivalPipeline)
        {
            _retrivalPipeline = retrivalPipeline;
        }
        public async Task<IActionResult> RetrivalGenerationCall(string prompt)
        {
            var relevantChunks = await _retrivalPipeline.GetRelevantChunks(prompt);

            return Ok(relevantChunks);
        }       
    }
}
