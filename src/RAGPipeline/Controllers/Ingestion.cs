using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace RAGPipeline.Controllers
{
    public class Ingestion : Controller
    {
        private readonly IngestionPipeline _ingestionPipeline;

        public Ingestion(IngestionPipeline ingestionPipeline)
        {
            _ingestionPipeline = ingestionPipeline;
        }
        public async Task<IActionResult> IngestText([FromForm] string text)
        {
            await _ingestionPipeline.IngestData(text);
            return Ok();
        }
    }
}
