using GameTools.Framework.Concepts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.API.WorkloadProvider.AiWorkloads
{
    public class GeneratedCharacterProperties
    {

        public GeneratedCharacterProperties()
        {
            Name = new GeneratedProperty();
            Appearance = new GeneratedProperty();
            Personality = new GeneratedProperty();
            Background = new GeneratedProperty();
            CurrentCircumstances = new GeneratedProperty();
        }

        public GeneratedProperty Appearance { get; set; }

        public GeneratedProperty Name { get; private set; }

        public GeneratedProperty Personality { get; private set; }

        public GeneratedProperty Background { get; private set; }

        public GeneratedProperty CurrentCircumstances { get; private set; }
    }
}
