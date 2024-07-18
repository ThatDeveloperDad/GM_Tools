using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.AIWorkloadManager.Contracts
{
    /// <summary>
    /// Exposes Predefined AI Workloads to other services in a software system.
    /// These workloads are invoked by ID (name), with any data required by the
    /// requested workload.
    /// </summary>
    public interface IAiWorkloadManager
    {
        string TestLLM();

        void RegisterAiFunction(AiFunctionDefinition aiFunction);

        Task<AiFunctionResult> ExecuteFunctionAsync(string functionName, Dictionary<string, object?> arguments);

        
    }
}
