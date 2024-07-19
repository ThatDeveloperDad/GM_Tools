using ThatDeveloperDad.LlmAccess.Contracts;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace ThatDeveloperDad.LlmAccess
{
    public class LlmProvider : ILlmProvider
    {
        public async Task<FunctionResponse> ExecuteFunctionAsync(FunctionRequest request)
        {
            FunctionResponse response = new FunctionResponse(request);

            try
            {
                Kernel kernel = BuildKernel();
                var toExecute = BuildFunction(request);
                var arguments = ConvertRequestParams(request);

                var functionResult = await toExecute.InvokeAsync(kernel, arguments);

                var theAnswer = functionResult.ToString();
                response.Result = theAnswer;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                response.AddErrorMessage(message);
            }

            return response;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> ExecutePromptAsync(string prompt)
        {
            //TODO:  Move this hardcoded stuff out to configuration.
#pragma warning disable SKEXP0010
            Kernel kernel = BuildKernel();

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

        #region private methods

        private Kernel BuildKernel()
        {
            Kernel kernel = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(
                    modelId: "phi3:mini",
                    endpoint: new Uri("http://localhost:1234"),
                    apiKey: string.Empty)
                .Build();
            return kernel;
        }

        private KernelFunction BuildFunction(FunctionRequest request)
        {
            string template = request.Function.Template;
            string name = request.Function.Name;
            string description = request.Function.Description;

            //TODO:  This is smelly.  We ought to abstract this later.
            PromptExecutionSettings settings = new()
            {
                ExtensionData = new Dictionary<string, object>()
                {
                    { "Temperature", 0.7 },
                    { "MaxTokens", 500 }
                }
            };

            KernelFunction func = KernelFunctionFactory.CreateFromPrompt(template,
                functionName: name,
                description: description,
                executionSettings: settings);
            
            return func;
        }

        private KernelArguments ConvertRequestParams(FunctionRequest request)
        {
            KernelArguments args = new KernelArguments();

            foreach(string argName in request.Parameters.Keys)
            {
                if (request.Parameters[argName] != null)
                {
                    args.Add(argName, request.Parameters[argName]);
                }
            }

            return args;
        }

        #endregion //private methods
    }
}
