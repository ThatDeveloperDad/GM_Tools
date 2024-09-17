using GameTools.MeteredUsageAccess.SqlServer.Context.SqlModels;
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

        public DbSet<QuotaTemplateSqlModel> QuotaTemplates { get; set; }

        public DbSet<UserQuotaSqlModel> UserQuotas { get; set; }

        public DbSet<UserSubscriptionSqlModel> UserSubscriptions { get; set; }

        /// <summary>
        /// This table exists purely for monitoring purposes.
        /// </summary>
        public DbSet<TokenConsumptionSqlModel> TokenConsumptions { get; set; }

    }
}
