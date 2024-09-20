using GameTools.NPCAccess.SqlServer.Context.SqlModels;
using Microsoft.EntityFrameworkCore;
using System;

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
            optionsBuilder.UseSqlServer(_cn,
				sqlOptions => sqlOptions.EnableRetryOnFailure(
					maxRetryCount: 5,
					maxRetryDelay: TimeSpan.FromSeconds(30),
					errorNumbersToAdd: null));
        }

        public DbSet<NpcRowModel> Npcs { get; set; }
    }
}
