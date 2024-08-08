using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GameTools.NPCAccess.SqlServer.Context
{
    internal class NpcAccessDbContext : DbContext
    {
        protected NpcAccessDbContext(string cn)
        {

        }
    }
}
