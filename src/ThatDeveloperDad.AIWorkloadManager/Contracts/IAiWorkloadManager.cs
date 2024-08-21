using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatDeveloperDad.Framework.Wrappers;

namespace ThatDeveloperDad.AIWorkloadManager.Contracts
{
    /// <summary>
    /// Exposes Predefined AI Workloads to other services in a software system.
    /// These workloads are invoked by ID (name), with any data required by the
    /// requested workload.
    /// </summary>
    public interface IAiWorkloadManager
    {
        /// <summary>
        /// Allows a component or service to register an AI
        /// function with the AI Workload Manager so that it can be
        /// executed as needed, later.
        /// </summary>
        /// <param name="aiFunction"></param>
        void RegisterAiFunction(AiFunctionDefinition aiFunction);

        /// <summary>
        /// Executes a previously Registered AI Function by name,
        /// providing the required argument values.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        Task<OpResult<AiFunctionResult>> ExecuteFunctionAsync(string functionName, Dictionary<string, object?> arguments);   
    }
}
