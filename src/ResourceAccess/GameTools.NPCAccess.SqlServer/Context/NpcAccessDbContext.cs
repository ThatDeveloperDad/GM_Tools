using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameTools.NPCAccess.SqlServer.Context.SqlModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace GameTools.NPCAccess.SqlServer.Context
{
    internal class NpcAccessDbContext : DbContext
    {
        private readonly string _cn;
        public NpcAccessDbContext(string cn):base()
        {
            _cn = cn;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_cn);
        }

        public DbSet<NpcRowModel> Npcs { get; set; }
    }
}
