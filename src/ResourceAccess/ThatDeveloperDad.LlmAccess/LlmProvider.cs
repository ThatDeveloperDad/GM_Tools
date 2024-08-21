using ThatDeveloperDad.LlmAccess.Contracts;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using ThatDeveloperDad.Framework.Wrappers;

namespace ThatDeveloperDad.LlmAccess
{
    public class LlmProvider : ILlmProvider
    {

        private readonly SemanticKernelConfiguration _lmConfig;

        public LlmProvider(SemanticKernelConfiguration lmConfig)
        {
            _lmConfig = lmConfig;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<OpResult<LlmResponse>> ExecuteFunctionAsync(LlmRequest request)
        {
            OpResult<LlmResponse> providerResult = new OpResult<LlmResponse>();
            LlmResponse aiResponse = new LlmResponse(request);

            providerResult.Payload = aiResponse;

            try
            {
                Kernel kernel = BuildKernel();
                var toExecute = BuildFunction(request);
                var arguments = ConvertRequestParams(request);

                var functionResult = await toExecute.InvokeAsync(kernel, arguments);

                var theAnswer = functionResult.ToString();
                var usageData = QueryResultForUsage(functionResult);

                providerResult.Payload.Result = theAnswer;
                providerResult.Payload.TokenUsage = usageData;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                providerResult.AddError(Guid.NewGuid(), message);
            }

            return providerResult;
        }

        #region private methods

        private Kernel BuildKernel()
        {
#pragma warning disable SKEXP0010
			Kernel kernel = Kernel.CreateBuilder()
						 .AddOpenAIChatCompletion(
							 modelId: _lmConfig.ModelId,
							 endpoint: new Uri(_lmConfig.EndpointUrl),
							 apiKey: _lmConfig.ApiKey)
						 .Build();
			return kernel;
        }

        private KernelFunction BuildFunction(LlmRequest request)
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

        private KernelArguments ConvertRequestParams(LlmRequest request)
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

        private AiUsage QueryResultForUsage(FunctionResult result)
        {
            int tokensIn = 0;
            int tokensOut = 0;

            var metadata = result.Metadata;
            object? usageData = null;
            metadata?.TryGetValue("Usage", out usageData);

            if (usageData != null)
            {
                var completionsUsage = usageData as Azure.AI.OpenAI.CompletionsUsage;
                tokensIn = completionsUsage?.PromptTokens??default(int);
                tokensOut = completionsUsage?.CompletionTokens ?? default(int);
			}

            AiUsage usage = new AiUsage
                (
                    modelId: _lmConfig.ModelId,
                    inputTokens: tokensIn,
                    outputTokens: tokensOut
                );

            return usage;
        }

		#endregion //private methods
	}
}
