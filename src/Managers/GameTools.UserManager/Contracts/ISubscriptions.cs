using ThatDeveloperDad.Framework.Wrappers;

namespace GameTools.UserManager.Contracts
{
	/// <summary>
	/// Defines the operataions required to manipulate user subscriptions.
	/// </summary>
	public interface ISubscriptions
	{
		/// <summary>
		/// Invoked when a new Subscription is created for a User.
		/// 
		/// Creates a record for the subscription that holds Kind,
		/// Start & End dates, and whether it has been Discontinued.
		/// </summary>
		/// <returns></returns>
		Task<OpResult<Subscription>> StartSubscription(string userId, string? subscriptionKind = null);

		/// <summary>
		/// Retrieves the Subscription data for a particular user.
		/// 
		/// This data will also include a report of their ResourceQuotas with
		/// the current consumption amounts listed.
		/// </summary>
		/// <returns></returns>
		Task<OpResult<Subscription>> LoadSubscription(string userId);

		/// <summary>
		/// Updates the Subscription row for the identified user
		/// Adds the Period time to the PeriodEnd field.
		/// 
		/// Also updates any Quotas that need to be refreshed when 
		/// the Subscription Period flips to a new one.
		/// </summary>
		/// <returns></returns>
		Task<OpResult<Subscription>> RenewSubscription(string userId);

		/// <summary>
		/// Sets the UnsubscribedDate on the user's subscription record
		/// to the date that the Cancel request arrived.
		/// 
		/// If the CancelNow flag is passed as true, the PeriodEnd for that user's
		/// subscription is moved to that same timestamp.
		/// </summary>
		/// <returns></returns>
		Task<OpResult<Subscription>> CancelSubscription(string userId, bool cancelImmediately = false);

	}
}
