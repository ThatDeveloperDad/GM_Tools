using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.AIWorkloadManager.Contracts
{
    public class AiFunctionResult
    {
        public AiFunctionResult(AiFunctionDefinition functionDefinition)
        {
            FunctionDefinition = functionDefinition;
        }

        public AiFunctionDefinition? FunctionDefinition { get; set; }

        public string? AiResponse { get; set; }

        public AiConsumption? Consumption { get; set; }
    }
}
