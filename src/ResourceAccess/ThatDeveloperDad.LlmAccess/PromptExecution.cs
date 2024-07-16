using ThatDeveloperDad.LlmAccess.Contracts;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;


namespace ThatDeveloperDad.LlmAccess
{
    public class PromptExecution : IPromptExecution
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> ExecutePromptAsync(string prompt)
        {
#pragma warning disable SKEXP0010
            Kernel kernel = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(
                    modelId: "phi3:mini",
                    endpoint: new Uri("http://localhost:1234"),
                    apiKey: string.Empty)
                .Build();

            var aiService = kernel.GetRequiredService<IChatCompletionService>();
            var chatHistory = new ChatHistory();

            
            chatHistory.Add(new ChatMessageContent(AuthorRole.User, prompt));

            string aiResponse = string.Empty;

            await foreach(var item in
                aiService.GetStreamingChatMessageContentsAsync(chatHistory))
            {
                aiResponse += item.Content;
            }
            return aiResponse;

        }
    }
}
