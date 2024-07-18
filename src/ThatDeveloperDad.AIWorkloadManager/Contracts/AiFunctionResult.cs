using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.AIWorkloadManager.Contracts
{
    public class AiFunctionResult
    {
        private readonly List<string> _errors;

        public AiFunctionResult(string error)
        {
            _errors = new List<string>();
            _errors.Add(error);
        }

        public AiFunctionResult(AiFunctionDefinition functionDefinition)
        {
            FunctionDefinition = functionDefinition;
            _errors = new List<string>();
        }

        public AiFunctionDefinition? FunctionDefinition { get; set; }

        public bool IsSuccessful => _errors.Any() == false;

        public string[] Errors => _errors.ToArray();

        public void AddError(string error)
        {
            _errors.Add(error);
        }

        public string? AiResponse { get; set; }
    }
}
