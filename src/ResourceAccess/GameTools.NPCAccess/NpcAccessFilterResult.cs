using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.NPCAccess
{
    public record NpcAccessFilterResult
    {
        public NpcAccessFilterResult():this(default(int), string.Empty, string.Empty, string.Empty) { }

        public NpcAccessFilterResult(int id, string species, string vocation, string name)
        {
            Id = id;
            Species = species;
            Vocation = vocation;
            Name = name;
        }

        public int Id { get; init; }

        public string Species { get; init; }

        public string Vocation { get; init; }

        public string Name { get; set; }
    }
}
