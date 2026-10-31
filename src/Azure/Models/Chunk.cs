namespace Azure.Models
{
    public class Chunk
    {
        public required string ChunkId { get; set; }

        public required string ActualText { get; set; }

        public required float[] Embedding { get; set; }

        public required DateTime CreatedOn { get; set; }
    }
}
