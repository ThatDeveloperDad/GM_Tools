using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.TownsfolkManager.Contracts
{
    /// <summary>
    /// Represents one NPC that matches the user provided filter criteria.
    /// </summary>
    public record FilteredTownsperson
    {
        public FilteredTownsperson(int npcId, string name, string species, string vocation)
        {
            NpcId = npcId;
            Name = name;
            Species = species;
            Vocation = vocation;
        }

        public  int NpcId { get; init; }

        public string Name { get; init; }

        public string Species { get; init; }

        public string Vocation { get; init; }
    }
}
