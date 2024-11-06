using Azure.Core.Extensions;
using DotNetEnv;
using GameTools.NPCAccess;
using GameTools.NPCAccess.AzureTables;
using GameTools.NPCAccess.SqlServer;
using Microsoft.Extensions.Configuration;

namespace TableStorageTest
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var config = LoadConfig();
			INpcAccess tableStorage = CreateNpcTableProvider(config);
			INpcAccess sqlStorage = GetNpcSqlProvider(config);
			string userId = "a0b66013-a5ef-462f-a812-3eb4aeacff66";
			NpcAccessFilter npcFilter = new()
			{
				UserId = userId
			};

			//var tableOpResult = await tableStorage.FilterNpcs(npcFilter);
			var sqlOpResult = await sqlStorage.FilterNpcs(new NpcAccessFilter());

			var sqlNpcs = sqlOpResult.Payload;
			foreach(var npcHeader in sqlNpcs)
			{
				var sqlNpcOp = await sqlStorage.LoadNpc(npcHeader.Id);
				var sqlNpc = sqlNpcOp.Payload;

				var atsNpcOp = await tableStorage.SaveNpc(sqlNpc, sqlNpc.UserId);
				var atsNpcId = atsNpcOp.Payload;
				Console.WriteLine($"Migrated {sqlNpc.CharacterName} to table storage:  ID = {atsNpcId}");
			}
			
		}

		static INpcAccess CreateNpcTableProvider(IConfiguration config)
		{
			var configSection = config.GetRequiredSection("ATS");
			string cn = configSection["ConnectionString"];

			INpcAccess service = new NpcAccessAzureTableProvider(cn);
			return service;
		}

		static INpcAccess GetNpcSqlProvider(IConfiguration config)
		{
			var configSection = config.GetRequiredSection("SQL");
			string cn = configSection["ConnectionString"];

			INpcAccess service = new NpcAccessSqlProvider(null, cn);
			return service;
		}

		static IConfiguration LoadConfig()
		{
			Env.Load();
			var builder = new ConfigurationBuilder()
				.AddEnvironmentVariables();

			return builder.Build();
		}
	}
}
