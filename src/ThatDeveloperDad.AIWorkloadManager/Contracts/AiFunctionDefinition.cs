using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.AIWorkloadManager.Contracts
{
    public class AiFunctionDefinition
    {

        public AiFunctionDefinition(string name, string description, string template)
        {
            Name = name;
            Description = description;
            Template = template;
        }

        public string Name { get; set;  }

        public string Description { get; set; }

        public string Template { get; set; }
    }
}
