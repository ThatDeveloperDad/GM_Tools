using GameTools.MeteredUsageAccess;
using GameTools.MeteredUsageAccess.ResourceModels;
using GameTools.UserManager.Contracts;
using GameTools.UserManager.InternalOperations;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using ThatDeveloperDad.Framework.Wrappers;

namespace GameTools.UserManager
{
	public class UsageManager : IUsageMeters
	{
		private readonly IQuotaAccess _quotaAccess;
		private readonly ILogger<UsageManager>? _logger;

        public UsageManager(IQuotaAccess quotaAccess,
			ILogger<UsageManager>? logger)
        {
            _quotaAccess = quotaAccess;
			_logger = logger;
        }

        public Task<OpResult<QuotaContainer>> ConsumeQuotaAsync(int quotaId, int amountConsumed)
		{
			throw new NotImplementedException();
		}

		public async Task<OpResult<QuotaContainer>> LoadUserQuotaAsync(string userId)
		{
			OpResult<QuotaContainer> mgrResult = new OpResult<QuotaContainer>();
			QuotaContainer? quotaContainer = null;

			try
			{
				var accessResult = await _quotaAccess.LoadUserQuotaAsync(userId);
				if(accessResult !=null 
					&& accessResult.WasSuccessful
					&& accessResult.Payload != null)
				{
					quotaContainer = accessResult
										.Payload
										.ToAppModel();
				}
				else
				{
					accessResult?.CopyErrorsTo(ref mgrResult);
				}

			}
			catch (Exception ex)
			{
				Guid exId = Guid.NewGuid();
				string message = $"An error occurred while loading the quotas for user {userId}";
				mgrResult.AddError(exId, message);

				_logger?.LogError(ex, exId.ToString());
			}

			mgrResult.Payload = quotaContainer;
			return mgrResult;
		}

		public Task<OpResult<QuotaContainer>> ReleaseQuotaAsync(int quotaId, int amounReleased)
		{
			throw new NotImplementedException();
		}
	}
}
