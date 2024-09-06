using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.SqlServer.Context
{
    internal class UsageAccessDbContext:DbContext
    {
        private readonly string _cn;

        public UsageAccessDbContext(string cn):base()
        {
            _cn = cn;            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_cn);
        }

    }
}
