using ThatDeveloperDad.AIWorkloadManager.Contracts;
using ThatDeveloperDad.LlmAccess.Contracts;

namespace ThatDeveloperDad.AIWorkloadManager
{
    public class AiWorkloadManager : IAiWorkloadManager
    {
        private readonly IPromptExecution _llmProvider;

        public AiWorkloadManager(IPromptExecution llmProvider)
        {
            _llmProvider = llmProvider;
        }

        public string DescribeNPC(string npcJson)
        {
            throw new NotImplementedException();
        }

        public string TestLLM()
        {

            string simplePrompt = "How many days are in a week?";
            string result = _llmProvider.ExecutePromptAsync(simplePrompt)
                                        .Result;

            return result;
        }
    }
}
