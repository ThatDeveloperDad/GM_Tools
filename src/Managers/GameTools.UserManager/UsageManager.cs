using GameTools.MeteredUsageAccess;
using GameTools.UserManager.Contracts;
using GameTools.UserManager.InternalOperations;
using Microsoft.Extensions.Logging;
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

        public async Task<OpResult<QuotaContainer>> ConsumeQuotaAsync(int quotaId, int amountConsumed, string userId)
		{
			OpResult<QuotaContainer> mgrResult = new OpResult<QuotaContainer>();
			QuotaContainer? mgrPayload = null;

			try
			{
				var accessResult = await  _quotaAccess.ConsumeQuotaAsync(quotaId, amountConsumed, userId);
				if (accessResult.WasSuccessful && accessResult.Payload != null)
				{
					mgrPayload = accessResult.Payload.ToAppModel();
				}
				else
				{
					accessResult.CopyErrorsTo(ref mgrResult);
				}
			}
			catch(Exception ex)
			{
				Guid exId = Guid.NewGuid();
				string message = $"An error occurred while updating quotaId {quotaId}";
				mgrResult.AddError(exId, message);
				_logger?.LogError(ex, exId.ToString());
			}

			mgrResult.Payload = mgrPayload;
			return mgrResult;
		}

		public async Task<OpResult<QuotaContainer>> LoadUserQuotaAsync(string userId)
		{
			OpResult<QuotaContainer> mgrResult = new OpResult<QuotaContainer>();
			QuotaContainer? quotaContainer = null;

			try
			{
				var accessResult = await _quotaAccess.LoadUserQuotaAsync(userId);
				if(accessResult.WasSuccessful && accessResult.Payload != null)
				{
					quotaContainer = accessResult.Payload.ToAppModel();
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

		public async Task<OpResult<QuotaContainer>> ReleaseQuotaAsync(int quotaId, int amountReleased, string userId)
		{
			OpResult<QuotaContainer> mgrResult = new OpResult<QuotaContainer>();
			QuotaContainer? mgrPayload = null;

			try
			{
				var accessResult = await _quotaAccess.ReleaseQuotaAsync(quotaId, amountReleased, userId);
				if(accessResult.WasSuccessful && accessResult.Payload != null)
				{
					mgrPayload = accessResult.Payload.ToAppModel();
				}
				else
				{
					accessResult.CopyErrorsTo(ref mgrResult);
				}
			}
			catch(Exception ex)
			{
				Guid exId = Guid.NewGuid();
				string message = $"An error occurred while Releasing resources on quotaId {quotaId}";
				mgrResult.AddError(exId, message);

				_logger?.LogError(ex, exId.ToString());
			}

			mgrResult.Payload = mgrPayload;
			return mgrResult;
		}

		public async Task<OpResult> LogTokenConsumption(TokenUsageEntry usageEntry)
		{
			OpResult mgrResult = new OpResult();

			try
			{
				var accessEntry = usageEntry.ToResourceModel();
				var accessResult = await _quotaAccess.LogTokenConsumption(accessEntry);
				if(accessResult.WasSuccessful == false)
				{
					accessResult.CopyErrorsTo(ref mgrResult);
				}
			}
			catch(Exception ex)
			{
				Guid exId = Guid.NewGuid();
				string message = $"An error occurred while logging tokens used during {usageEntry.FunctionName} for userId {usageEntry.UserId}";
				mgrResult.AddError(exId, message);

				_logger?.LogError(ex, exId.ToString());
			}

			return mgrResult;
		}
	}
}
