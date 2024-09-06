using GameTools.MeteredUsageAccess.ResourceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatDeveloperDad.Framework.Wrappers;

namespace GameTools.MeteredUsageAccess
{
    /// <summary>
    /// Describes the UserSubscription and Quota Access tasks that are invoked by
    /// a subscription management component.
    /// </summary>
    public interface IUserSubscriptionAccess
    {
        /// <summary>
        /// Invoked when a new Subscription is created for a User.
        /// 
        /// Creates a record for the subscription that holds Kind,
        /// Start & End dates, and whether it has been Discontinued.
        /// </summary>
        /// <returns></returns>
        Task<OpResult<UserSubscription>> StartSubscription(string userId, string? subscriptionKind = null);

        /// <summary>
        /// Retrieves the Subscription data for a particular user.
        /// 
        /// This data will also include a report of their ResourceQuotas with
        /// the current consumption amounts listed.
        /// </summary>
        /// <returns></returns>
        Task<OpResult<UserSubscription>> LoadSubscription(string userId);

        /// <summary>
        /// Updates the Subscription row for the identified user
        /// Adds the Period time to the PeriodEnd field.
        /// 
        /// Also updates any Quotas that need to be refreshed when 
        /// the Subscription Period flips to a new one.
        /// </summary>
        /// <returns></returns>
        Task<OpResult<UserSubscription>> RenewSubscription(string userId);

        /// <summary>
        /// Sets the UnsubscribedDate on the user's subscription record
        /// to the date that the Cancel request arrived.
        /// 
        /// If the CancelNow flag is passed as true, the PeriodEnd for that user's
        /// subscription is moved to that same timestamp.
        /// </summary>
        /// <returns></returns>
        Task<OpResult<UserSubscription>> CancelSubscription(string userId, bool cancelImmediately = false);

    }
}
