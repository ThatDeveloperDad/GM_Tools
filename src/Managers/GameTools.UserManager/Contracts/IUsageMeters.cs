using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatDeveloperDad.Framework.Wrappers;

namespace GameTools.UserManager.Contracts
{
	public interface IUsageMeters
	{
		OpResult<QuotaContainer> LoadUserQuota(string userId) => LoadUserQuotaAsync(userId).Result;

		/// <summary>
		/// Loads the usage quotas for a particular user.
		/// </summary>
		/// <returns>The a Task containing User's Quota object w/ their current consumption data</returns>
		Task<OpResult<QuotaContainer>> LoadUserQuotaAsync(string userId);

		OpResult<QuotaContainer> ConsumeQuota(int quotaId, int amountConsumed) => ConsumeQuotaAsync(quotaId, amountConsumed).Result;

		/// <summary>
		/// Updates the consumed quota for a specific user, resource, and time period.
		/// 
		/// Use this when a new character is generated or saved.
		/// </summary>
		/// <returns>The a Task containing The User's Quota object w/ the updated consumption data</returns>
		Task<OpResult<QuotaContainer>> ConsumeQuotaAsync(int quotaId, int amountConsumed);

		OpResult<QuotaContainer> ReleaseQuota(int quotaId, int amounReleased) => ReleaseQuotaAsync(quotaId, amounReleased).Result;

		/// <summary>
		/// Updates a specific quota for User + Resource when an 
		/// instance of that resource is released.
		/// 
		/// Dont' use this for AI Consumption. (Can't put the cookie back in the jar after you've swallowed it.)
		/// BUT, If an NPC is deleted, that will release a StorageConsumption unit.
		/// </summary>
		/// <returns>The a Task containing Returns the User's Quota object with the updated consumption data.</returns>
		Task<OpResult<QuotaContainer>> ReleaseQuotaAsync(int quotaId, int amounReleased);

	}
}
