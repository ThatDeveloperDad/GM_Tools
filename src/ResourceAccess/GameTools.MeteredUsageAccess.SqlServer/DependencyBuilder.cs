using GameTools.MeteredusageAccess.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.SqlServer
{
    public static class DependencyBuilder
    {
        private const string RequiredCnString = "userdata";

        public static IServiceCollection UseQuotaAccessSqlProvider(this IServiceCollection services,
            IConfiguration configuration)
        {
            string? cn = configuration.GetConnectionString(RequiredCnString);
            if(cn == null)
            {
                throw new InvalidOperationException("Cannot start the system without the userdata connection string.");
            }

            services.AddScoped<IQuotaAccess>((sp) =>
            {
                ILoggerFactory? logFactory = sp.GetRequiredService<ILoggerFactory>();
                ILogger<QuotaAccessSqlProvider>? logger = logFactory?.CreateLogger<QuotaAccessSqlProvider>();

                QuotaAccessSqlProvider svc
                    = new QuotaAccessSqlProvider(cn, logger);

                return svc;
            });

            return services;
        }

        public static IServiceCollection UseUserSubscriptionSqlProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            string? cn = configuration.GetConnectionString(RequiredCnString);
            if(cn == null)
            {
                throw new InvalidOperationException("Cannot start the application without the required connection string.");
            }

            services.AddScoped<IUserSubscriptionAccess>((sp) =>
            {
                ILoggerFactory? lf = sp.GetRequiredService<ILoggerFactory>();
                ILogger<UserSubscriptionAccessSqlProvider>? logger = lf?.CreateLogger<UserSubscriptionAccessSqlProvider>();
                IUserSubscriptionAccess svc = new UserSubscriptionAccessSqlProvider(cn, logger);

                return svc;
            });

            return services;
        }
    }
}
