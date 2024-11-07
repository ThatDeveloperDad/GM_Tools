using GameTools.MeteredUsageAccess;
using GameTools.MeteredUsageAccess.ResourceModels;
using GameTools.UserAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableStorageTest.LocalServices.Transformers;

namespace TableStorageTest.LocalServices
{
	internal class UserAccountBuilder
	{
		private readonly IUserAccess _userAccess;
		private readonly IUserSubscriptionAccess _subAccess;

		public UserAccountBuilder(IUserAccess userAccess,
			IUserSubscriptionAccess subAccess) 
		{
			_userAccess = userAccess;
			_subAccess = subAccess;
		}

		public async Task<UserAccount?> BuildUserAccount(string userId)
		{
			UserAccount? account = null;

			try
			{
				// Fetch the user's identity information that we can from UserAccess
				var userIdentity = await _userAccess.LoadAppUserAsync(userId);
				if (userIdentity == null)
				{
					Console.WriteLine($"User {userId} not found in the Identity Store.");
					return null;
				}
				account = new()
				{
					UserId = userIdentity.UserId,
					DisplayName = userIdentity.DisplayName
				};
				account.ExternalIds
					.Add
					(
						new UserVendorId()
						{
							UserIdAtVendor = userId,
							VendorName = _userAccess.IdentityVendor
						}
					);

				// See if the user has a subscription in the SubscriptionStore.
				UserSubscription? subResource = null;
				var subResult = await _subAccess.LoadSubscription(userId);
				if(subResult.WasSuccessful)
				{
					subResource = subResult.Payload;
					Subscription? accountSub = subResource.ToSubDto();
					if(accountSub != null)
					{
						account.Subscription = accountSub;
						account.SubscriptionStatus = accountSub.CurrentStatus;
					}
				}

			}
			catch(Exception ex)
			{
				Console.WriteLine($"Error building user account: {ex.Message}");
			}

			return account;
		}
	}
}
