using GameTools.MeteredUsageAccess.ResourceModels;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableStorageTest.LocalServices.Transformers
{
	internal static class ModelExtensions
	{
		public static Subscription? ToSubDto(this UserSubscription? resource)
		{
			Subscription? dto = null;

			if(resource != null)
			{
				dto = new();
				dto.UserId = resource.UserId;
				dto.WillRenew = resource.WillRenew;
				dto.StartDateUtc = resource.StartDateUtc;
				dto.EndDateUtc = resource.EndDateUtc;
				dto.UserQuotas.UserId = resource.UserId;
				dto.UserQuotas.Storage.Budget = resource.Quotas.Storage.Budget;
				dto.UserQuotas.Storage.Consumption = resource.Quotas.Storage.Consumption;
				dto.UserQuotas.NpcAiGeneration.Budget = resource.Quotas.AiGenerations.Budget;
				dto.UserQuotas.NpcAiGeneration.Consumption = resource.Quotas.AiGenerations.Consumption;

				//TODO:  This might need to move into a Subscription Engine, especially if we 
				// have additional "Calculated" Subscription attributes.
				dto.CurrentStatus = "None";
				DateTime moment = DateTime.UtcNow;
				if(resource.StartDateUtc <= moment 
					&& resource.EndDateUtc >= moment)
				{
					if(resource.WillRenew == true)
					{
						if(resource.EndDateUtc>=moment)
						{
							dto.CurrentStatus = "Active";
						}
						else
						{
							dto.CurrentStatus = "Renewing";
						}
					}
					else
					{
						dto.CurrentStatus = "Expiring";
					}
				}
				else
				{
					dto.CurrentStatus = "Inactive";
				}
			}

			return dto;
		}

		public static UserResource ToUserResource(this UserAccount dto)
		{
			UserResource resource = new UserResource();

			resource.UserId = dto.UserId;
			resource.DisplayName = dto.DisplayName;
			resource.SubscriptionStatus = dto.SubscriptionStatus;

			resource.CurrentSubscription = dto.Subscription.ToResourceModel();

			foreach(UserVendorId idDto in dto.ExternalIds)
			{
				UserIdResource? idResource = idDto.ToResourceModel();
				if(idResource != null)
				{
					resource.Ids.Add(idResource);
				}
			}

			return resource;
		}

		public static UserSubscription? ToResourceModel(this Subscription? dto)
		{
			UserSubscription? resource = new UserSubscription();
			if(dto == null)
			{
				return null;
			}


			resource.UserId = dto.UserId;
			resource.WillRenew = dto.WillRenew;
			resource.StartDateUtc = dto.StartDateUtc;
			resource.EndDateUtc = dto.EndDateUtc;
			resource.Quotas.Storage.QuotaId = dto.UserQuotas.Storage.QuotaId;
			resource.Quotas.Storage.Budget = dto.UserQuotas.Storage.Budget;
			resource.Quotas.Storage.Consumption = dto.UserQuotas.Storage.Consumption;
			resource.Quotas.AiGenerations.QuotaId = dto.UserQuotas.NpcAiGeneration.QuotaId;
			resource.Quotas.AiGenerations.Budget = dto.UserQuotas.NpcAiGeneration.Budget;
			resource.Quotas.AiGenerations.Consumption = dto.UserQuotas.NpcAiGeneration.Consumption;
			resource.CurrentStatus = dto.CurrentStatus;

			return resource;
		}

		public static UserIdResource? ToResourceModel(this UserVendorId dto)
		{
			UserIdResource? resource = new UserIdResource();
			if (dto == null)
			{
				return null;
			}

			resource.UserIdAtVendor = dto.UserIdAtVendor;
			resource.VendorName = dto.VendorName;

			return resource;
		}
	}
}
