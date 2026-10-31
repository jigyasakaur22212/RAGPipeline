using Microsoft.Extensions.AI;
using OpenAI.Embeddings;
using System;
using System.Collections.Generic;
using System.Text;

namespace LLM.Services
{
    public class Embedding
    {
        private IEmbeddingGenerator<string, Embedding<float>> _generator;
        public Embedding()
        {
            _generator = new EmbeddingClient(Environment.GetEnvironmentVariable("OpenApi:EmbeddingModelName"), Environment.GetEnvironmentVariable("OpenApi:ApiKey"))
                .AsIEmbeddingGenerator();

        }

        public async Task<float> GenerateEmbeddingAsync(string text)
        {
            var embeddings = await _generator.GenerateAsync(text);
            return embeddings.Vector.ToArray()[0];
        }
    }
}
