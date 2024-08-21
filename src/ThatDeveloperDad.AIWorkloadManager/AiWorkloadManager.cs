﻿using ThatDeveloperDad.AIWorkloadManager.Contracts;
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

        public async Task<AiFunctionResult> ExecuteFunctionAsync(string functionName, Dictionary<string, object?> arguments)
        {
            AiFunctionDefinition? storedFunction =  GuardFunctionExists(functionName);

            if (storedFunction == null)
            {
                string error = $"The requested function ({functionName}) is not registered";
                return new AiFunctionResult(error);
            }

            var funcDef = storedFunction!;
            AiFunctionResult result = new AiFunctionResult(funcDef);

            var request = new FunctionRequest();
            // set up the request.
            request.Function.Name = funcDef.Name;
            request.Function.Description = funcDef.Description;
            request.Function.Template = funcDef.Template;

            foreach(var entry in arguments)
            {
                request.Parameters.Add(entry.Key, entry.Value);
            }

            // send the Request to the LLM Provider
            var aiResult = await _llmProvider.ExecuteFunctionAsync(request);

            // Process the result.
            if(aiResult.IsSuccessful == false)
            {
                foreach (var error in aiResult.Errors)
                {
                    result.AddError(error);
                }
            }
            result.AiResponse = aiResult.Result;
            result.Consumption = aiResult.TokenUsage?.ToLocalModel();
            return result;
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
