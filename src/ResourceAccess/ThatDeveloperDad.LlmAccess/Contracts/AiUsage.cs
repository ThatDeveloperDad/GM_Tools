using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.LlmAccess.Contracts
{
	public record AiUsage
	{
        public AiUsage(string? modelId, int inputTokens, int outputTokens)
        {
            ModelId = modelId;
            PromptTokens = inputTokens;
            ResponseTokens = outputTokens;
        }

        public string? ModelId { get; set; }

        public int PromptTokens { get; set; }

        public int ResponseTokens { get; set; }
    }
}
