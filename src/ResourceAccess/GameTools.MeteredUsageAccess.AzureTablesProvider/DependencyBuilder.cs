using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.AzureTablesProvider
{
	public static class DependencyBuilder
	{
		private const string RequiredCnString = "AzureTableStorage";

		public static IServiceCollection UseQuotaAccessTableProvider
			(this IServiceCollection services,
			 IConfiguration configuration)
		{
			var cn = configuration.GetConnectionString(RequiredCnString);
			if(cn == null)
			{
				throw new InvalidOperationException("Cannot start the system without the userdata connection string.");
			}

			services.AddScoped<IQuotaAccess>((sp) =>
			{
				ILoggerFactory? lf = sp.GetRequiredService<ILoggerFactory>();
				ILogger<QuotaAccessTableProvider>? logger = lf?.CreateLogger<QuotaAccessTableProvider>();

				QuotaAccessTableProvider svc = new QuotaAccessTableProvider(cn, logger);
				return svc;

			});

			return services;
		}

		public static IServiceCollection UseSubscriptionAccessTableProvider
			(this IServiceCollection services,
			 IConfiguration configuration)
		{
			var cn = configuration?.GetConnectionString(RequiredCnString);
			if (cn == null)
			{
				throw new InvalidOperationException("Cannot start the system without the userdata connection string.");
			}

			services.AddScoped<IUserSubscriptionAccess>((sp) =>
			{
				ILoggerFactory? lf = sp.GetRequiredService<ILoggerFactory>();
				ILogger<UserSubscriptionTableProvider>? logger = lf?.CreateLogger<UserSubscriptionTableProvider>();

				UserSubscriptionTableProvider svc = new UserSubscriptionTableProvider(cn, logger);
				return svc;
			});

			return services;
		}

		public static IQuotaAccess BuildQuotaAccess(IConfiguration config)
		{
			var configSection = config.GetRequiredSection("ATS");
			var cn = configSection["ConnectionString"];

			var svc = new QuotaAccessTableProvider(cn, null);
			return svc;
		}

		public static IUserSubscriptionAccess BuildUserSubscriptionAccess(IConfiguration config)
		{
			var configSection = config.GetRequiredSection("ATS");
			var cn = configSection["ConnectionString"];

			var svc = new UserSubscriptionTableProvider(cn, null);
			return svc;
		}
	}
}
