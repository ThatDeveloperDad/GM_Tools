using GameTools.Ruleset.Definitions;
using GameTools.Ruleset.Definitions.Characters;
using GameTools.TownsfolkManager.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.TownsfolkManager.InternalOperations
{
    internal static class TemplateWorker
    {
        public static Townsperson ApplySpecies(this Townsperson npc, SpeciesTemplate template)
        {
            npc.Species = template.Name;
            return npc;
        }

        public static Townsperson ApplyBackground(this Townsperson npc, BackgroundTemplate template)
        {
            npc.Background.Name = template.Name;
            return npc; 
        }

        public static Townsperson ApplyVocation(this Townsperson npc, VocationTemplate template)
        {
            npc.Vocation.Name = template.Name;
            return npc; 
        }
    }
}
