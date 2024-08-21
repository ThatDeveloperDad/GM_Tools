using ThatDeveloperDad.AIWorkloadManager.Contracts;
using ThatDeveloperDad.Framework.Wrappers;
using ThatDeveloperDad.LlmAccess.Contracts;

namespace ThatDeveloperDad.AIWorkloadManager
{
    public class AiWorkloadManager : IAiWorkloadManager
    {
        private readonly ILlmProvider _llmProvider;
        private readonly Dictionary<string, AiFunctionDefinition> _functions;

        public AiWorkloadManager(ILlmProvider llmProvider)
        {
            _llmProvider = llmProvider;
            _functions = new Dictionary<string, AiFunctionDefinition>();
        }

        public async Task<OpResult<AiFunctionResult>> ExecuteFunctionAsync(string functionName, Dictionary<string, object?> arguments)
        {
            OpResult<AiFunctionResult> aiManagerResult = new OpResult<AiFunctionResult>();

            AiFunctionDefinition? storedFunction =  GuardFunctionExists(functionName);

            if (storedFunction == null)
            {
                string error = $"The requested function ({functionName}) is not registered";
                aiManagerResult.AddError(Guid.NewGuid(), error);
                
                return aiManagerResult;
            }

            var funcDef = storedFunction!;
            AiFunctionResult result = new AiFunctionResult(funcDef);
            aiManagerResult.Payload = result;

            var request = new LlmRequest();
            // set up the request.
            request.Function.Name = funcDef.Name;
            request.Function.Description = funcDef.Description;
            request.Function.Template = funcDef.Template;

            foreach(var entry in arguments)
            {
                request.Parameters.Add(entry.Key, entry.Value);
            }

            // send the Request to the LLM Provider
            var aiProviderResult = await _llmProvider.ExecuteFunctionAsync(request);

            // Process the result.
            if(aiProviderResult.WasSuccessful == false)
            {
                foreach (var kvp in aiProviderResult.Errors)
                {
                    aiManagerResult.AddError(kvp.Key, kvp.Value);
                }
            }
            else
            {
                string? aiResult = aiProviderResult.Payload?.Result;
                var usage = aiProviderResult.Payload?.TokenUsage?.ToLocalModel();

				aiManagerResult.Payload.AiResponse = aiResult;
				aiManagerResult.Payload.Consumption = usage;
			}

			return aiManagerResult;
        }

        public void RegisterAiFunction(AiFunctionDefinition aiFunction)
        {
            _functions[aiFunction.Name] = aiFunction;
        }

        #region private methods

        private AiFunctionDefinition? GuardFunctionExists(string functionName)
        {
            if(_functions.ContainsKey(functionName))
            {
                return _functions[functionName];
            }

            return null;
        }

        #endregion
    }
}
