using GameTools.MeteredUsageAccess.ResourceModels;
using ThatDeveloperDad.Framework.Wrappers;

namespace GameTools.MeteredUsageAccess
{
    /// <summary>
    /// Because Storage and Language Model access in the cloud are
    /// neither free, nor cheap enough to count as part of Cost of Goods Sold,
    /// We need to monitor and limit the usage by our subscribers of
    /// these resources.
    /// 
    /// IQuotaAccess provides the methods by which the main Application will manage a user's
    /// quotas during the execution of User-initiated Requests.
    /// </summary>
    public interface IQuotaAccess
    {

        OpResult<UserQuota> LoadUserQuota(string userId) => LoadUserQuotaAsync(userId).Result;

        /// <summary>
        /// Loads the usage quotas for a particular user.
        /// </summary>
        /// <returns>The a Task containing User's Quota object w/ their current consumption data</returns>
        Task<OpResult<UserQuota>> LoadUserQuotaAsync(string userId);

        OpResult<UserQuota> ConsumeQuota(int quotaId, int amountConsumed) => ConsumeQuotaAsync(quotaId, amountConsumed).Result;

        /// <summary>
        /// Updates the consumed quota for a specific user, resource, and time period.
        /// 
        /// Use this when a new character is generated or saved.
        /// </summary>
        /// <returns>The a Task containing The User's Quota object w/ the updated consumption data</returns>
        Task<OpResult<UserQuota>> ConsumeQuotaAsync(int quotaId, int amountConsumed);

        OpResult<UserQuota> ReleaseQuota(int quotaId, int amounReleased) => ReleaseQuotaAsync(quotaId, amounReleased).Result;

        /// <summary>
        /// Updates a specific quota for User + Resource when an 
        /// instance of that resource is released.
        /// 
        /// Dont' use this for AI Consumption. (Can't put the cookie back in the jar after you've swallowed it.)
        /// BUT, If an NPC is deleted, that will release a StorageConsumption unit.
        /// </summary>
        /// <returns>The a Task containing Returns the User's Quota object with the updated consumption data.</returns>
        Task<OpResult<UserQuota>> ReleaseQuotaAsync(int quotaId, int amounReleased);

    }
}
