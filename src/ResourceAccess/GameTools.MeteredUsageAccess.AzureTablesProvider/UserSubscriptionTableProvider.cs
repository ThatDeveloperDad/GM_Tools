using Azure;
using Azure.Data.Tables;
using GameTools.MeteredUsageAccess.AzureTablesProvider.StorageModels;
using GameTools.MeteredUsageAccess.AzureTablesProvider.Transformers;
using GameTools.MeteredUsageAccess.ResourceModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatDeveloperDad.Framework.Wrappers;

namespace GameTools.MeteredUsageAccess.AzureTablesProvider
{
	internal class UserSubscriptionTableProvider : IUserSubscriptionAccess
	{
		private readonly string _connectionString;
		private readonly TableServiceClient _storageClient;
		private readonly ILogger? _logger;

        public UserSubscriptionTableProvider(string cn,
			ILogger<UserSubscriptionTableProvider>? logger)
        {
			_connectionString = cn;
			_logger = logger;

			_storageClient = new TableServiceClient(_connectionString);
        }

		public async Task<UserResource?> SaveUserAccount(UserResource userAccount)
		{
			UserResource? resource = null;

			try
			{
				UserEntity? entity = null;
				TableClient client = BuildTableClient(UserEntity.BaseTableName);
				try
				{
					var tableQuery = await client
					.GetEntityAsync<UserEntity>(UserEntity.TablePartitionId, userAccount.UserId);
					entity = tableQuery.Value;
				}
				catch (RequestFailedException ex404)
				{
					// This is expected if the entity doesn't exist.
					// Eat error & continue.
				}
				
				if(entity == null)
				{
					entity = userAccount.ToUserEntity();
				}
				else
				{
					entity = entity.ApplyUserChanges(userAccount);
				}

				if(entity != null)
				{
					var response = await client.UpsertEntityAsync(entity);
					if(response.Status >= 400)
					{
						var message = response.ReasonPhrase;
						throw new Exception($"The Storage operation failed. {message}");
					}
					if(response.Status >= 200 && response.Status < 300)
					{
						resource = entity.ToUserDto();
					}
				}	
			}
			catch (Exception ex)
			{
				Guid errorId = Guid.NewGuid();
				_logger?.LogError(ex, $"Error saving user account. Error ID: {errorId}");
				throw;
			}

			return resource;
		}


        public Task<OpResult<UserSubscription>> CancelSubscription(string userId, bool cancelImmediately = false)
		{
			throw new NotImplementedException();
		}

		public Task<OpResult<UserSubscription>> LoadSubscription(string userId)
		{
			throw new NotImplementedException();
		}

		public Task<OpResult<UserSubscription>> RenewSubscription(string userId)
		{
			throw new NotImplementedException();
		}

		public Task<OpResult<UserSubscription>> StartSubscription(string userId, string? subscriptionKind = null)
		{
			throw new NotImplementedException();
		}

		private TableClient BuildTableClient(string tableName)
		{
			_storageClient.CreateTableIfNotExists(tableName);
			TableClient tableClient = _storageClient.GetTableClient(tableName);
			return tableClient;
		}
	}
}
