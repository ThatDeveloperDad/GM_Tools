using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.TownsfolkManager.Contracts
{
    public record TownspersonFilter
    {
        public string? Species { get; set; }

        public string? Vocation { get; set; }
    }
}
