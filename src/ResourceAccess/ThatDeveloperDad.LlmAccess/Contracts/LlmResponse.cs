using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.LlmAccess.Contracts
{
    public class LlmResponse
    {
       
        public LlmResponse(LlmRequest request) 
        {
            Request = request;
            Result = string.Empty;
        }

        public LlmRequest Request { get; private set; }

        public string Result { get; set; }

        public AiUsage? TokenUsage { get; set; }
    }
}
