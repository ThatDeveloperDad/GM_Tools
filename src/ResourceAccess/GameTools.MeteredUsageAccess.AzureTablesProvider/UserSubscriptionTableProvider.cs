using GameTools.MeteredUsageAccess.ResourceModels;
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
	}
}
