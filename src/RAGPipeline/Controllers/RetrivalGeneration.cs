using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace RAGPipeline.Controllers
{
    [ApiController]
    [Route("api/retrieval")]
    public class RetrivalGeneration : ControllerBase
    {
        private readonly IRetrivalPipeline _retrivalPipeline;

        public RetrivalGeneration(IRetrivalPipeline retrivalPipeline)
        {
            _retrivalPipeline = retrivalPipeline;
        }

        [HttpPost]
        public async Task<IActionResult> Retrival([FromBody] string prompt)
        {
            var answer = await _retrivalPipeline.GenerateAnswerAsync(prompt);
            return Ok(new { answer });
        }
    }
}
