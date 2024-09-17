using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.NPCAccess
{
    public record NpcAccessFilter
    {
        public string? UserId { get; set; }

        public string? Species { get; set; }

        public string? Vocation { get; set; }
    }
}
