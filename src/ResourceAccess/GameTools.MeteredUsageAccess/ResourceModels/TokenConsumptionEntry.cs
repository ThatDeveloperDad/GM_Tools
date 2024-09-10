using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.ResourceModels
{
    public class TokenConsumptionEntry
    {
        public TokenConsumptionEntry()
        {
            FunctionName = string.Empty;
            UserId = string.Empty;
            modelId = string.Empty;
        }

        public string UserId { get; init; }
        public DateTime InferenceTimeUtc { get; init; }
        public string FunctionName { get; init; }
        public int PromptTokens { get; init; }
        public int CompletionTokens { get; init; }
        public string modelId { get; init; }
    }
}
