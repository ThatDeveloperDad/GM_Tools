using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.LlmAccess.Contracts
{
    /// <summary>
    /// Allows us to request that a function be executed by the SementicKernel
    /// </summary>
    public class LlmRequest
    {
        public LlmRequest() 
        {
            Function = new SemanticDefinition();
            Parameters = new Dictionary<string, object?>();
        }

        public SemanticDefinition Function { get; init; }

        public Dictionary<string, object?> Parameters { get; init; }
    }
}
