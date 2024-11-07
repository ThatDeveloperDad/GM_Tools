using GameTools.MeteredUsageAccess.AzureTablesProvider.StorageModels;
using GameTools.MeteredUsageAccess.ResourceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatDeveloperDad.Framework.Serialization;

namespace GameTools.MeteredUsageAccess.AzureTablesProvider.Transformers
{
	internal static class UsageDataTransformers
	{
		public static TokenConsumptionEntity ToEntity(this TokenConsumptionEntry dto)
		{
			TokenConsumptionEntity entity = new();
			entity.modelId = dto.modelId;
			entity.UserId = dto.UserId;
			entity.FunctionName = dto.FunctionName;
			entity.PromptTokens = dto.PromptTokens;
			entity.CompletionTokens = dto.CompletionTokens;
			entity.InferenceTimeUtc = dto.InferenceTimeUtc;

			return entity;
		}

		public static TokenConsumptionEntry ToDto(this TokenConsumptionEntity entity)
		{
			TokenConsumptionEntry dto = new()
			{
				UserId = entity.UserId,
				FunctionName = entity.FunctionName,
				PromptTokens = entity.PromptTokens,
				CompletionTokens = entity.CompletionTokens,
				InferenceTimeUtc = entity.InferenceTimeUtc,
				modelId = entity.modelId
			};

			return dto;
		}

		public static UserEntity ToUserEntity(this UserResource dto)
		{
			UserEntity entity = new();

			entity.UserId = dto.UserId;
			entity.DisplayName = dto.DisplayName;
			entity.SubscriptionStatus = dto.SubscriptionStatus;
			entity.IdsJson = dto.Ids.SerializeForStorage()??string.Empty;
			entity.CurrentSubscriptionJson = dto.CurrentSubscription?.SerializeForStorage()??string.Empty;
			entity.SubscriptionActivityJson = dto.SubscriptionActivity?.SerializeForStorage()??string.Empty;
			return entity;
		}

		public static UserResource? ToUserDto(this UserEntity? entity)
		{
			if (entity == null)
				return null;

			UserResource dto = new();
			dto.UserId = entity.UserId;
			dto.DisplayName = entity.DisplayName;
			dto.SubscriptionStatus = entity.SubscriptionStatus;

			if (string.IsNullOrWhiteSpace(entity.IdsJson) == false)
			{
				dto.Ids = entity.IdsJson!
					.ToInstance<List<UserIdResource>>() 
					?? new List<UserIdResource>();
			}

			if(string.IsNullOrWhiteSpace(entity.CurrentSubscriptionJson) == false)
			{
				UserSubscription sub = entity.CurrentSubscriptionJson
					.ToInstance<UserSubscription>()
					?? new UserSubscription();
				dto.CurrentSubscription = sub;
			}

			if(string.IsNullOrWhiteSpace(entity.SubscriptionActivityJson) == false)
			{
				List<SubscriptionActivityResource> list 
					= entity.SubscriptionActivityJson
					  .ToInstance<List<SubscriptionActivityResource>>()
					  ?? new List<SubscriptionActivityResource>();

				dto.SubscriptionActivity = list;
			}

			return dto;
		}

		public static UserEntity ApplyUserChanges(this UserEntity entity, UserResource dto)
		{
			entity.UserId = dto.UserId;
			entity.DisplayName = dto.DisplayName;
			entity.SubscriptionStatus =	dto.SubscriptionStatus;
			entity.IdsJson = dto.Ids.SerializeForStorage()??string.Empty;
			entity.CurrentSubscriptionJson = dto.CurrentSubscription?.SerializeForStorage()??string.Empty;
			entity.SubscriptionActivityJson = dto.SubscriptionActivity?.SerializeForStorage()??string.Empty;

			return entity;
		}
	}
}
