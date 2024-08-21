using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace GameTools.NPCAccess.SqlServer
{
	public static class DependencyHelper
    {
        public static IServiceCollection UseNpcSqlServerAccess(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            string? userdataCn = configuration.GetConnectionString("userdata");
            if(string.IsNullOrWhiteSpace(userdataCn))
            {
                throw new ArgumentNullException("Could not access the expected Connection String (userdata) from the supplied configuration.");
            }

            services.AddScoped<INpcAccess>((sp) => 
                {
                    ILoggerFactory logFactory = sp.GetRequiredService<ILoggerFactory>();
                    ILogger<NpcAccessSqlProvider> npcLogger = logFactory.CreateLogger<NpcAccessSqlProvider>();
                    return new NpcAccessSqlProvider(npcLogger, userdataCn);
                });

            return services;
        }
    }
}
