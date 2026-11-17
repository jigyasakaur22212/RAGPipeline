using OpenAI;
using OpenAI.Chat;

namespace LLM.Services
{
    public class LLMGeneration : ILLMGeneration
    {
        private readonly ChatClient _chatClient;

        public LLMGeneration()
        {
            var apiKey = Environment.GetEnvironmentVariable("OpenAI:ApiKey")
                ?? Environment.GetEnvironmentVariable("OpenApi:ApiKey");
            var model = Environment.GetEnvironmentVariable("OpenAI:Model")
                ?? Environment.GetEnvironmentVariable("OpenApi:ChatModelName");

            _chatClient = new OpenAIClient(apiKey).GetChatClient(model);
        }

        public async Task<string> GenerateAnswerAsync(string userPrompt, IReadOnlyList<string> chunks)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userPrompt);

            var context = string.Join(Environment.NewLine + Environment.NewLine, chunks);
            var prompt = $"Use only the following context to answer the user's question. If the answer is not present in the context, say that you do not have enough information.\n\nContext:\n{context}\n\nUser question:\n{userPrompt}";
            var completion = await _chatClient.CompleteChatAsync(prompt);

            return completion.Value.Content.Count == 0
                ? "The language model returned an empty response."
                : completion.Value.Content[0].Text;
        }
    }
}
