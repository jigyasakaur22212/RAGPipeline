using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    public class ChunkingData
    {
        private readonly int _chunkSize;

        public ChunkingData()
        {
            _chunkSize = int.Parse(Environment.GetEnvironmentVariable("ChunkSize") ?? "1000");
        }
        public List<string> ChunkData(string data)
        {
            var chunks = new List<string>();
            for (int i = 0; i < data.Length; i += _chunkSize)
            {
                var chunkContent = data.Substring(i, Math.Min(_chunkSize, data.Length - i));
                chunks.Add(chunkContent);
            }
            return chunks;
        }
    }
}
