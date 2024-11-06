using Azure.Data.Tables;
using GameTools.MeteredUsageAccess.AzureTablesProvider.StorageModels;
using GameTools.MeteredUsageAccess.AzureTablesProvider.Transformers;
using GameTools.MeteredUsageAccess.ResourceModels;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;
using ThatDeveloperDad.Framework.Wrappers;

namespace GameTools.MeteredUsageAccess.AzureTablesProvider
{
	internal class QuotaAccessTableProvider : IQuotaAccess
	{
		private const string _TableName = "BusinessObjects";
		private const string _UsersPartitionKey = "Users";
		
		private readonly ILogger _logger;

		private readonly string _connectionString;
		private readonly TableServiceClient _storageClient;

        public QuotaAccessTableProvider(string connectionString,
			ILogger<QuotaAccessTableProvider>? _logger)
        {
			_connectionString = connectionString;
			_storageClient = new TableServiceClient(_connectionString);

			_storageClient.CreateTableIfNotExists(_TableName);
        }

        public async Task<OpResult<UserQuota>> ConsumeQuotaAsync(int quotaId, int amountConsumed, string? userId = "")
		{
			OpResult<UserQuota> result = new();
			UserQuota? payload = null;
			try
			{
				var user = await LoadUser(userId);

				var quota = user?.CurrentSubscription?.Quotas.GetQuota(quotaId);

				if(quota == null)
				{
					throw new ArgumentException($"Quota with Id {quotaId} could not be found on user {userId}.");
				}

				quota.Consumption = quota.Consumption + amountConsumed;

				user = await SaveUser(user);
				payload = user.CurrentSubscription.Quotas;
			}
			catch (Exception ex)
			{
				Guid errorId = Guid.NewGuid();
				_logger?.LogError(ex, errorId.ToString());
				string errorMessage = $"Could not consume the user quota. {ex.Message}";
				result.AddError(errorId, errorMessage);
			}

			result.Payload = payload;
			return result;
		}

		public async Task<OpResult<UserQuota>> LoadUserQuotaAsync(string userId)
		{
			OpResult<UserQuota> result = new();
			UserQuota? payload = null;

			try
			{
				payload = await LoadUserQuota(userId);
			}
			catch (Exception ex)
			{
				Guid errorId = Guid.NewGuid();
				_logger?.LogError(ex, errorId.ToString());
				string errorMessage = $"Could not load the user quota. {ex.Message}";
				result.AddError(errorId, errorMessage);
			}
			
			result.Payload = payload;
			return result;
		}

		public async Task<OpResult> LogTokenConsumption(TokenConsumptionEntry meterEntry)
		{
			OpResult opResult = new();

			try
			{
				var table = BuildTableClient(TokenConsumptionEntity.BaseTableName);

				var entity = meterEntry.ToEntity();

				await table.UpsertEntityAsync(entity);
			}
			catch(Exception ex)
			{
				Guid errorId = Guid.NewGuid();
				_logger?.LogError(ex, errorId.ToString());
				string errorMessage = $"Could not log the consumption record. {ex.Message}";
				opResult.AddError(errorId, errorMessage);
			}

			return opResult;
		}

		public async Task<OpResult<UserQuota>> ReleaseQuotaAsync(int quotaId, int amountReleased, string? userId = "")
		{
			OpResult<UserQuota> result = new();
			UserQuota? payload = null;
			try
			{
				var user = await LoadUser(userId);

				var quota = user?.CurrentSubscription?.Quotas.GetQuota(quotaId);

				if (quota == null)
				{
					throw new ArgumentException($"Quota with Id {quotaId} could not be found on user {userId}.");
				}

				quota.Consumption = quota.Consumption - amountReleased;

				user = await SaveUser(user);
				payload = user.CurrentSubscription.Quotas;
			}
			catch (Exception ex)
			{
				Guid errorId = Guid.NewGuid();
				_logger?.LogError(ex, errorId.ToString());
				string errorMessage = $"Could not consume the user quota. {ex.Message}";
				result.AddError(errorId, errorMessage);
			}

			result.Payload = payload;
			return result;
		}

		private async Task<UserResource?> LoadUser(string userId) 
		{
			var table = BuildTableClient(UserEntity.BaseTableName);

			var userEntity = 
				await table.GetEntityAsync<UserEntity>(UserEntity.TablePartitionId, userId);

			return userEntity.Value.ToUserDto();
        }

		private async Task<UserQuota> LoadUserQuota(string userId)
		{
			UserResource? user = await LoadUser(userId);
			if (user == null)
			{
				throw new ArgumentException($"User with Id {userId} could not be loaded.");
			}

			if (user.CurrentSubscription == null)
			{
				throw new ArgumentException($"User with Id {userId} does not have a subscription.");
			}

			return user.CurrentSubscription.Quotas;
		}

		private async Task<UserResource?> SaveUser(UserResource user)
		{
			var table = BuildTableClient(UserEntity.BaseTableName);
			
			UserEntity userEntity = new();
			var userEntityCall =
				await table.GetEntityAsync<UserEntity>(UserEntity.TablePartitionId, user.UserId);
			if (userEntityCall.Value != null)
			{
				userEntity = userEntityCall.Value;
			}

			userEntity = userEntity.ApplyUserChanges(user);

			await table.UpsertEntityAsync(userEntity);

			return userEntity.ToUserDto();

		}

		private TableClient BuildTableClient(string tableName)
		{
			_storageClient.CreateTableIfNotExists(tableName);
			TableClient tableClient = _storageClient.GetTableClient(tableName);
			return tableClient;
		}
	}
}
