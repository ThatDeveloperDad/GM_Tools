using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatDeveloperDad.Framework.Guards;

namespace GameTools.UserAccess.MsGraphProvider
{
	public static class DependencyBuilder
	{
		private const string MissingConfigMessage = "A required configuration section is missing.  Cannot bootstrap the application.";
		

		public static GraphConfiguration LoadMsGraphConfiguration(
			this IConfiguration config,
			string? hostEnv = null)
		{
			var cfg = new GraphConfiguration();

			//if ((hostEnv??"Development") == "Production")
			//{
			//	cfg.TenantId = config[GraphConfiguration.EnvVar_TenantId];
			//	cfg.ClientId = config[GraphConfiguration.EnvVar_ClientId];
			//	cfg.ClientSecret = config[GraphConfiguration.EnvVar_ClientSecret];
			//	cfg.ApplicationGroupPrefix = config[GraphConfiguration.EnvVar_AppGrpPrefix];
			//}
			//else 
			//{
				var cfgNode = config.GetRequiredSection(GraphConfiguration.SettingNode);

				cfgNode.GuardNullReference(MissingConfigMessage);

				cfg.TenantId = cfgNode[GraphConfiguration.SettingKey_TenantId];
				cfg.ClientId = cfgNode[GraphConfiguration.SettingKey_ClientId];
				cfg.ClientSecret = cfgNode[GraphConfiguration.SettingKey_ClientSecret];
				cfg.ApplicationGroupPrefix = cfgNode[GraphConfiguration.SettingKey_AppGrpPrefix];
			//}

			cfg.TenantId.GuardNullReference("TenantId is required to start the app.  Check the configuration settings or Environment Variables.");
			cfg.ClientId.GuardNullReference("ClientId is required to start the app.   Check the configuration settings or Environment Variables.");
			cfg.ClientSecret.GuardNullReference("ClientSecret is required to start the app.   Check the configuration settings or Environment Variables.");

			return cfg;
		}

		public static IServiceCollection UseMsGraphUserProvider
			(this IServiceCollection services,
			 GraphConfiguration gphCfg)
		{
			services.AddScoped<IUserAccess>((sp) =>
			{
				ILoggerFactory? lf = sp.GetRequiredService<ILoggerFactory>();
				ILogger<UserAccessGraphProvider>? logger = lf?.CreateLogger<UserAccessGraphProvider>();

				var userProvider = new UserAccessGraphProvider(gphCfg, logger);

				return userProvider;
			});


			return services;
		}
	}
}
