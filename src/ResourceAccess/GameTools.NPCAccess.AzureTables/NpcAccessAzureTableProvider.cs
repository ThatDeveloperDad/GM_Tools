using Azure.Data.Tables;
using GameTools.NPCAccess.AzureTables.StorageModels;
using GameTools.NPCAccess.AzureTables.Transformers;
using ThatDeveloperDad.Framework.Wrappers;

namespace GameTools.NPCAccess.AzureTables
{
	public class NpcAccessAzureTableProvider : INpcAccess
	{
		private const string BaseTableName = "UserObjects";
		
		private readonly string _connectionString;
		private readonly TableServiceClient _storageClient;

        public NpcAccessAzureTableProvider(string connectionString)
        {
            _connectionString = connectionString;
			_storageClient = new TableServiceClient(_connectionString);
        }

        public async Task<OpResult<IEnumerable<NpcAccessFilterResult>>> FilterNpcs(NpcAccessFilter filter)
		{
			OpResult<IEnumerable<NpcAccessFilterResult>> opResult = new();
			List<NpcAccessFilterResult> filterResults = new();

			string partitionKey = CalculatePartitionKey(NpcTableEntity.PartitionKeyPrefix ,filter.UserId);
			try
			{
				// First, we obtain a TableClient from the TableServiceClient.  The Table we want
				// is called "UserObjects".
				TableClient userObjectClient = BuildTableClient();

				
				// query the table for all entities that match the partition key.
				// This gets all NPCs that belong to the user identified in the Filter.
				var npcEntities = userObjectClient.Query<NpcTableEntity>
					(
						e => e.PartitionKey == partitionKey
					).ToList();

				foreach (var npcEntity in npcEntities.OrderByDescending(e=> e.CreatedDate))
				{
					NpcAccessFilterResult filterResult = new()
					{
						Id = npcEntity.NpcId,
						Name = npcEntity.CharacterName ?? string.Empty,
						Species = npcEntity.SpeciesName,
						Vocation = npcEntity.VocationName,
					};
					filterResults.Add(filterResult);
				}
			}
			catch(Exception ex)
			{
				Guid errorId = Guid.NewGuid();
				string errorMessage = ex.Message;
				opResult.AddError(errorId, errorMessage);
			}
			
			opResult.Payload = filterResults;

			return opResult;
		}

		public async Task<OpResult<NpcAccessModel?>> LoadNpc(int npcId, string? userId = "")
		{
			OpResult<NpcAccessModel?> opResult = new();
			NpcAccessModel? npcAccessModel = null;

			string partitionKey = CalculatePartitionKey(NpcTableEntity.PartitionKeyPrefix, userId);

			try
			{
				TableClient userObjects = BuildTableClient();

				var entityQuery = userObjects.QueryAsync<NpcTableEntity>
					(
						e => e.PartitionKey == partitionKey
						  && e.NpcId == npcId
					);

				await foreach (NpcTableEntity storedNpc in entityQuery)
				{
					if(storedNpc.NpcId == npcId && storedNpc.UserId == userId)
					{
						npcAccessModel = storedNpc.ToDto();
						break;
					}
				}

			}
			catch(Exception ex)
			{
				Guid errorId = Guid.NewGuid();
				string errorMessage = ex.Message;
				opResult.AddError(errorId, errorMessage);
			}

			opResult.Payload = npcAccessModel;
			return opResult;
		}

		public async Task<OpResult<int>> SaveNpc(NpcAccessModel npc, string? userId = null)
		{
			OpResult<int> opResult = new();
			int? npcId = npc.NpcId;

			string partitionKey = CalculatePartitionKey(NpcTableEntity.PartitionKeyPrefix, userId);

			try
			{
				TableClient userObjects = BuildTableClient();
				NpcTableEntity? entity = null;
				if(npc.NpcId != null)
				{
					entity = userObjects
					.Query<NpcTableEntity>
					(
						e => e.PartitionKey == partitionKey
						  && e.NpcId == npc.NpcId
					).FirstOrDefault();
				}
				
				if (entity == null)
				{
					var inboundId = npc.NpcId??1;

					if (npc.NpcId.HasValue == false)
					{
						var userNpcs = await FilterNpcs(new NpcAccessFilter { UserId = npc.UserId });
						if (userNpcs.Payload?.Any() ?? false)
						{
							inboundId = userNpcs.Payload.Max(m => m.Id) + 1;
						}
					}
					
					entity = new NpcTableEntity();
					entity.PartitionKey = partitionKey;
					entity.RowKey = Guid.NewGuid().ToString();
					entity.NpcId = inboundId;
					entity.UserId = userId;
					entity.SpeciesName = npc.Species;
					entity.VocationName = npc.Vocation;
					entity.CharacterName = npc.CharacterName;
					entity.CharacterDetails = npc.CharacterDetails;
					entity.IsPublic = npc.IsPublic;
					entity.CreatedDate = DateTime.UtcNow;
					entity.UpdatedDate = DateTime.UtcNow;

					var response = await userObjects.UpsertEntityAsync(entity);
					//TODO:  Inspect the Response from the Upsert call
					// and handle accordingly.

					opResult.Payload = entity.NpcId;
				}
				else
				{
					entity.SpeciesName = npc.Species;
					entity.VocationName = npc.Vocation;
					entity.CharacterName = npc.CharacterName;
					entity.CharacterDetails = npc.CharacterDetails;
					entity.UpdatedDate = DateTime.UtcNow;

					var response = await userObjects.UpsertEntityAsync(entity);
					//TODO:  Inspect the Response from the Upsert call
					// and handle accordingly.
					
					opResult.Payload = entity.NpcId;
				}
			}
			catch(Exception ex)
			{
				Guid errorId = Guid.NewGuid();
				string errorMessage = ex.Message;
				opResult.AddError(errorId, errorMessage);
			}

			return opResult;
		}

		private TableClient BuildTableClient()
		{
			_storageClient.CreateTableIfNotExists(BaseTableName);
			var userObjectClient = _storageClient.GetTableClient(BaseTableName);

			return userObjectClient;
		}

		private string CalculatePartitionKey(string userObjectKind, string userId)
		{
			userId = userId.Replace("-", string.Empty);
			return $"{userObjectKind}_{userId}";
		}
	}
}
