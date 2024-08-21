using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.LlmAccess.Contracts
{
    public class FunctionResponse
    {
        private readonly List<string> _errors;

        public FunctionResponse(FunctionRequest request) 
        {
            Request = request;
            _errors = new List<string>();
        }

        public FunctionRequest Request { get; private set; }

        public bool IsSuccessful => _errors.Count == 0;

        public string[] Errors => _errors.ToArray();

        public string Result { get; set; }

        public AiUsage? TokenUsage { get; set; }

        public void AddErrorMessage(string message)
        {
            _errors.Add(message);
        }
    }
}
