using Azure.Core.Extensions;
using DotNetEnv;
using GameTools.MeteredUsageAccess;
using GameTools.MeteredUsageAccess.SqlServer;
using ATS_Users = GameTools.MeteredUsageAccess.AzureTablesProvider;
using GameTools.UserAccess;
using Graph = GameTools.UserAccess.MsGraphProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models.ExternalConnectors;
using TableStorageTest.LocalServices;
using GameTools.MeteredUsageAccess.ResourceModels;
using TableStorageTest.LocalServices.Transformers;

namespace TableStorageTest
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var config = LoadConfig();

			var graphCLient = GetMsGraphClient(config);
			var userSql = GetSubAccessSqlProvider(config);
			var userTable = GetSubAccessTableProvider(config);

			var users = await graphCLient.LoadAppUsersAsync("ae9b8ead-4c75-4d5b-a28a-628712770523");

			UserAccountBuilder builder 
				= new
				(
					graphCLient, 
					userSql
				);

			foreach (var user in users)
			{
				var userAccount = await builder.BuildUserAccount(user.UserId);
				if (userAccount != null)
				{
					string subDescription = userAccount.Subscription == null
						? "Doesn't have a Subscription" 
						: $"Has an {userAccount.SubscriptionStatus} Subscription.";
					Console.WriteLine($"User: {userAccount.DisplayName}");
					Console.WriteLine($"  {subDescription}");

					UserResource userResource = userAccount.ToUserResource();
					var savedUser = await userTable.SaveUserAccount(userResource);
					if (savedUser != null)
					{
						Console.WriteLine($"{userResource.DisplayName} saved Successfully.");
					}
				}
			}
		}

		static IConfiguration LoadConfig()
		{
			Env.Load();
			var builder = new ConfigurationBuilder()
				.AddEnvironmentVariables();

			return builder.Build();
		}

		static IUserSubscriptionAccess GetSubAccessTableProvider(IConfiguration config)
		{
			IUserSubscriptionAccess svc;

			svc = ATS_Users.DependencyBuilder.BuildUserSubscriptionAccess(config);

			return svc;
		}

		static IUserSubscriptionAccess GetSubAccessSqlProvider(IConfiguration config)
		{
			var sqlSettings = config.GetSection("SQL");
			string? cn = sqlSettings["ConnectionString"];
			if (cn == null)
			{
				throw new InvalidOperationException("Cannot start the application without the required connection string.");
			}


			ILoggerFactory? lf = null;
			ILogger<UserSubscriptionAccessSqlProvider>? logger = null;
			IUserSubscriptionAccess svc = new UserSubscriptionAccessSqlProvider(cn, logger);

			return svc;			
		}

		static IUserAccess GetMsGraphClient(IConfiguration configuration)
		{
			var gphCfg = Graph.DependencyBuilder.LoadMsGraphConfiguration(configuration);

			ILoggerFactory? lf = null;
			ILogger<Graph.UserAccessGraphProvider>? logger = lf?.CreateLogger<Graph.UserAccessGraphProvider>();

			var userProvider = new Graph.UserAccessGraphProvider(gphCfg, logger);

			return userProvider;
		}
	}
}
