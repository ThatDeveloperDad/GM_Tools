using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.AIWorkloadManager.Contracts
{
	public record AiConsumption
	{

        public AiConsumption(string? modelId, int inputTokens, int outputTokens)
        {
            ModelId = modelId;
            PromptTokens = inputTokens;
            CompletionTokens = outputTokens;
        }

        public string? ModelId { get; set; }

        public int PromptTokens { get; set; }

        public int CompletionTokens { get; set; }
    }
}
