using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.LlmAccess.Contracts
{
    public class SemanticDefinition
    {

        public SemanticDefinition()
        {
            Name = string.Empty;
            Description = string.Empty;
            Template = string.Empty;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Template { get; set; }
    }
}
