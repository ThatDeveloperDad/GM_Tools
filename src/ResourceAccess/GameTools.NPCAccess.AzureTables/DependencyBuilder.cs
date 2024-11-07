using Azure.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.NPCAccess.AzureTables
{
	public static class DependencyBuilder
	{
		public static IServiceCollection UseNpcTableStorageProvider(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			string? atsCn = configuration.GetConnectionString("AzureTableStorage");
			if(string.IsNullOrWhiteSpace(atsCn))
			{
				throw new ArgumentNullException("Could not access the expected Connection String (AzureTableStorage) from the supplied configuration.");
			}

			services.AddScoped<INpcAccess>((sp) =>
			{
				return new NpcAccessAzureTableProvider(atsCn);
			});

			return services;
		}
	}
}
