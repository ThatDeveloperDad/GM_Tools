using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.LlmAccess.Contracts
{
    public interface IPromptExecution
    {

        /// <summary>
        /// Executes a simple text prompt against a LanguageModel
        /// and returns the reply.
        /// </summary>
        /// <param name="prompt">THe User prompt to send</param>
        /// <returns>The Reply from the LLM</returns>
        Task<string> ExecutePromptAsync(string prompt);
    }
}
