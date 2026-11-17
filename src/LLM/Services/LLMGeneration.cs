using OpenAI;
using OpenAI.Chat;

namespace LLM.Services
{
    public class LLMGeneration
    {
        private ChatClient chat;
        public LLMGeneration()
        {
            var client = new OpenAIClient(Environment.GetEnvironmentVariable("OpenAI:ApiKey"));
            chat = client.GetChatClient(Environment.GetEnvironmentVariable("OpenAI:Model"));
        }
        public string GenerateAnswer(string userPrompt , List<string> chunks)
        {
            ChatCompletionOptions options = new ChatCompletionOptions
            {

            };
            chat.CompleteChatAsync
        }
    }
}
