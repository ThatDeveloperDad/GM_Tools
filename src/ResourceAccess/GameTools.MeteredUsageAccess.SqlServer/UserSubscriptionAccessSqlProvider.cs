using GameTools.Framework.Concepts;
using GameTools.MeteredUsageAccess.ResourceModels;
using GameTools.MeteredUsageAccess.SqlServer.Context;
using GameTools.MeteredUsageAccess.SqlServer.Context.SqlModels;
using GameTools.MeteredUsageAccess.SqlServer.Transformers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatDeveloperDad.Framework.Wrappers;

namespace GameTools.MeteredUsageAccess.SqlServer
{
    public class UserSubscriptionAccessSqlProvider : IUserSubscriptionAccess, IDisposable
    {
        private const int SubscriptionLengthDays = 30;
        private readonly string _cn;
        private readonly ILogger<UserSubscriptionAccessSqlProvider>? _logger;
        private UsageAccessDbContext? _ctx;
        private Dictionary<string, QuotaTemplateSqlModel>? _quotaTemplates;
        private readonly DateTime Forever = new DateTime(year: 2079, month: 6, day: 6);
        private readonly DateTime _today = DateTime.UtcNow.Date;

        private bool disposedValue;

        public UserSubscriptionAccessSqlProvider
            (
                string cn,
                ILogger<UserSubscriptionAccessSqlProvider>? logger = null
            )
        {
            _logger = logger;
            _cn = cn;
        }

        private UsageAccessDbContext Ctx
        {
            get
            {
                _ctx = _ctx ?? new UsageAccessDbContext(_cn);
                return _ctx;
            }
        }

        private Dictionary<string, QuotaTemplateSqlModel> Templates
        {
            get
            {
                if (_quotaTemplates == null)
                {
                    var today = DateTime.Now;
                    var rows = Ctx
                        .QuotaTemplates
                        .Where(t => (t.DeletedDateUtc ?? Forever) > today);

                    var dict = new Dictionary<string, QuotaTemplateSqlModel>();
                    foreach (var row in rows)
                    {
                        if (dict.ContainsKey(row.ResourceKind) == false)
                        {
                            dict.Add(row.ResourceKind, row);
                        }
                    }
                    _quotaTemplates = dict;
                }

                return _quotaTemplates;
            }
        }

        public async Task<OpResult<UserSubscription>> CancelSubscription(string userId, bool cancelImmediately = false)
        {
            OpResult<UserSubscription> result = new OpResult<UserSubscription>();
            DateTime now = DateTime.UtcNow;
            try
            {
                // First, let's get the Subscription.
                var sub = await LoadCurrentSub(userId);

                if(sub == null)
                {
                    Guid errId = Guid.NewGuid();
                    string msg = $"Could not find a current subscription for the user {userId}";
                    _logger?.LogWarning($"ErrorId: {errId}  Message: {msg}  Operation: CancelSubscription");
                    result.AddError(errId, msg);
                    return result;
                }

                // Update the subscription to expire at end of current period.
                sub.UnsubscribedDateUtc = now;

                // if we're cancelling now, do that and zero out any remaining AI Token Budget.
                if(cancelImmediately == true)
                {
                    sub.PeriodEndUtc = now;

                    int tokenQuotaTpltId = Templates[MeteredResourceKind.NpcAiDetail.ToString()].Id;
                    // Also need to Zero out any remaining token quota.
                    // (Set Budget = Consumed)
                    var quota = await Ctx
                        .UserQuotas
                        .FirstOrDefaultAsync(t => t.UserId == userId
                                              && t.QuotaTemplateId == tokenQuotaTpltId);
                    if(quota != null)
                    {
                        quota.UpdatedDateUtc = now;
                        quota.CurrentBudget = quota.ConsumedBudget;
                        Ctx.UserQuotas.Update(quota);
                    }
                }
                Ctx.UserSubscriptions.Update(sub);

                await Ctx.SaveChangesAsync();
            }
            catch(Exception e)
            {
                Guid errId = Guid.NewGuid();
                string msg = $"An error occurred while Canceling the subscription for user {userId}.  {e.Message}";
                _logger?.LogError(e, $"ErrorId: {errId}  Message: {msg}");
                result.AddError(errId, msg);
            }

            var reloadSubOp = await LoadSubscription(userId);
            if (reloadSubOp.WasSuccessful)
            {
                result.Payload = reloadSubOp.Payload;
            }
            else
            {
                foreach (var error in reloadSubOp.Errors)
                {
                    result.AddError(error.Key, error.Value);
                }
            }
            return result;
        }

        public async Task<OpResult<UserSubscription>> LoadSubscription(string userId)
        {
            OpResult<UserSubscription> result = new OpResult<UserSubscription>();
            UserSubscription? resultPayload = null;
            try
            {
                var sub = await LoadCurrentSub(userId, false);
                if (sub == null)
                {
                    Guid errId = Guid.NewGuid();
                    string msg = $"Load subscription for user {userId} failed.  No current Subscription found.";
                    result.AddError(errId, msg);
                    return result;
                }
                resultPayload = sub.ToDto();

                // Now go get the Quotas.
                var quotas = await Ctx
                    .UserQuotas
                    .Where(q => q.UserId == userId)
                    .AsNoTracking()
                    .ToListAsync();
                var tokenQuotaTemplate = Templates[MeteredResourceKind.NpcAiDetail.ToString()];
                var storageQuotaTemplate = Templates[MeteredResourceKind.NpcStorage.ToString()];

                ResourceQuota? storageQuota = quotas.ExtractQuotaOfKind(storageQuotaTemplate);
                ResourceQuota? tokenQuota = quotas.ExtractQuotaOfKind(tokenQuotaTemplate);

                if(storageQuota == null || tokenQuota == null)
                {
                    Guid errId = Guid.NewGuid();
                    string msg = $"Could not find either the Storage or AiToken Quota for user Id {userId}";
                    result.AddError(errId, msg);
                    _logger?.LogWarning($"Error Id: {errId}  Message: {msg}");
                    return result;
                }

                var userQuotas = new UserQuota()
                {
                    UserId = userId,
                    Storage = storageQuota,
                    AiGenerations = tokenQuota
                };

                resultPayload.Quotas = userQuotas;
            }
            catch(Exception e)
            {
                Guid exId = Guid.NewGuid();
                _logger?.LogError(e, exId.ToString());
                result.AddError(exId, $"There was an error loading the subscription for user {userId}");
            }

            result.Payload = resultPayload;
            return result;
        }

        public async Task<OpResult<UserSubscription>> RenewSubscription(string userId)
        {
            OpResult<UserSubscription> result = new OpResult<UserSubscription>();
            DateTime now = DateTime.UtcNow;

            try
            {
                var sub = await LoadCurrentSub(userId);
                if(sub == null)
                {
                    string msg = $"Could not find a current subscription for user id {userId}.";
                    Guid errId = Guid.NewGuid();
                    result.AddError(errId, msg);
                    _logger?.LogWarning($"ErrorId: {errId}.  Message:  {msg}");

                    return result;
                }
                // Adjust the PeriodEnd date on the user's subscription.
                if(sub.PeriodEndUtc>_today)
                {
                    sub.PeriodEndUtc = sub.PeriodEndUtc.AddDays(SubscriptionLengthDays);
                }
                else
                {
                    sub.PeriodEndUtc = _today.AddDays(SubscriptionLengthDays);
                }
                sub.UpdatedDateUtc = now;

                // Reset the Consumed Budget for AI Tokens to zero.
                var tokenTemplate = Templates[MeteredResourceKind.NpcAiDetail.ToString()];
                var tokenQuota = await Ctx
                    .UserQuotas
                    .FirstOrDefaultAsync(q => q.UserId == userId
                                          && q.QuotaTemplateId == tokenTemplate.Id);

                if(tokenQuota == null)
                {
                    tokenQuota = new UserQuotaSqlModel()
                    {
                        UserId = userId,
                        QuotaTemplateId = tokenTemplate.Id,
                        CurrentBudget =tokenTemplate.UnitsGranted,
                        ConsumedBudget = 0,
                        CreatedDateUtc = _today,
                    };
                    Ctx.UserQuotas.Add(tokenQuota);
                }
                else
                {
                    tokenQuota.ConsumedBudget = 0;
                    tokenQuota.UpdatedDateUtc = _today;
                    Ctx.UserQuotas.Update(tokenQuota);
                }

                await Ctx.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Guid entryId = Guid.NewGuid();
                string message = $"An error occurred while updating the subscription for user {userId}.";
                _logger?.LogError(e, entryId.ToString());
                result.AddError(entryId, message);
            }

            var loadSubOp = await LoadSubscription(userId);
            UserSubscription? resultPayload = loadSubOp.Payload;
            result.Payload = resultPayload;

            return result;
        }

        public async Task<OpResult<UserSubscription>> StartSubscription(string userId, string? subscriptionKind = null)
        {
            OpResult<UserSubscription> result = new OpResult<UserSubscription>();
            UserSubscription? resultPayload = null;

            try
            {
                
                DateTime now = DateTime.UtcNow;

                // Verify that the userId is not already attached to an existing
                // and active subscription.
                var currentSubscription = await LoadCurrentSub(userId);
                if (currentSubscription != null)
                {
                    result.AddError(Guid.NewGuid(), $"User with Id {userId} already has an active subscription.");
                    result.Payload = currentSubscription.ToDto();
                    return result;
                }

                // Create a UserSubscription SqlModel and write it to the database.
                UserSubscriptionSqlModel newSub = new UserSubscriptionSqlModel()
                {
                    UserId = userId,
                    CreatedDateUtc = now,
                    PeriodStartUtc = _today,
                    PeriodEndUtc = _today.AddDays(SubscriptionLengthDays),
                };

                Ctx.UserSubscriptions.Add(newSub);

                // Provision UserQuotas for the different Resources the subscription grants.
                var storageTemplate = Templates[MeteredResourceKind.NpcStorage.ToString()];
                var tokenTemplate = Templates[MeteredResourceKind.NpcAiDetail.ToString()];

                var storageQuota = await Ctx
                    .UserQuotas
                    .FirstOrDefaultAsync(s=> s.UserId == userId
                                          && s.QuotaTemplateId == storageTemplate.Id);
                var tokenQuota = await Ctx
                    .UserQuotas
                    .FirstOrDefaultAsync(t=> t.UserId == userId
                                          && t.QuotaTemplateId == tokenTemplate.Id);

                if(storageQuota == null)
                {
                    storageQuota = new UserQuotaSqlModel()
                    {
                        UserId = userId,
                        QuotaTemplateId = storageTemplate.Id,
                        CurrentBudget = storageTemplate.UnitsGranted,
                        ConsumedBudget = 0,
                        CreatedDateUtc = _today
                    };
                    Ctx.UserQuotas.Add(storageQuota);
                }
                else
                {
                    storageQuota.UpdatedDateUtc = now;
                    Ctx.UserQuotas.Update(storageQuota);
                }
                
                if(tokenQuota == null)
                {
                    tokenQuota = new UserQuotaSqlModel()
                    {
                        UserId = userId,
                        QuotaTemplateId = tokenTemplate.Id,
                        CurrentBudget = tokenTemplate.UnitsGranted,
                        ConsumedBudget = 0,
                        CreatedDateUtc = _today
                    };
                    Ctx.UserQuotas.Add(tokenQuota);
                }
                else
                {
                    tokenQuota.UpdatedDateUtc = now;
                    Ctx.UserQuotas.Update(tokenQuota);
                }

                await Ctx.SaveChangesAsync();

                resultPayload = newSub.ToDto();
                UserQuota userQuota = new UserQuota()
                {
                    UserId = userId,
                    AiGenerations = tokenQuota.ToDto(tokenTemplate)!,
                    Storage = storageQuota.ToDto(storageTemplate)!
                };
                resultPayload.Quotas = userQuota;
            }
            catch(Exception ex)
            {
                string error = $"Could not start the subscription for user id {userId}.  {ex.Message}";
                _logger?.LogError(ex, error);
                result.AddError(Guid.NewGuid(), error);
            }

            result.Payload = resultPayload;
            return result;
        }

        private async Task<UserSubscriptionSqlModel?> LoadCurrentSub(string userId, bool forUpdate = true)
        {
            var query = Ctx
                .UserSubscriptions
                .Where(us => us.UserId == userId);
                

            if(forUpdate == false)
            {
                query = query.AsNoTracking();
            }

            query = query.OrderByDescending(us => us.PeriodEndUtc);

			var currentSubscription = await query.FirstOrDefaultAsync();
            return currentSubscription;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _ctx?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

		public Task<UserResource> SaveUserAccount(UserResource user)
		{
			throw new NotImplementedException();
		}
	}
}
