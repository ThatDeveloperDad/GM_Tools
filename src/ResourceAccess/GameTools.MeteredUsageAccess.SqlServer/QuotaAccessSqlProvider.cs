using GameTools.MeteredUsageAccess;
using GameTools.MeteredUsageAccess.ResourceModels;
using GameTools.MeteredUsageAccess.SqlServer.Context;
using GameTools.MeteredUsageAccess.SqlServer.Context.SqlModels;
using GameTools.MeteredUsageAccess.SqlServer.Transformers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ThatDeveloperDad.Framework.Wrappers;

namespace GameTools.MeteredusageAccess.SqlServer
{
    /// <summary>
    /// Implements IQuotaAccess against SQL Server, using Entity Framework Core.
    /// </summary>
    public class QuotaAccessSqlProvider : IQuotaAccess, IDisposable
    {
        private readonly string _cn;
        private readonly ILogger<QuotaAccessSqlProvider>? _logger;
        private UsageAccessDbContext? _ctx;
        private bool disposedValue;
        private Dictionary<string, QuotaTemplateSqlModel>? _quotaTemplates;
        private readonly DateTime Forever = new DateTime(year: 2079, month: 6, day: 6);

        public QuotaAccessSqlProvider
            (
                string cn,
                ILogger<QuotaAccessSqlProvider>? logger = null
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
                        if(dict.ContainsKey(row.ResourceKind) == false)
                        {
                            dict.Add(row.ResourceKind, row);
                        }
                    }
                    _quotaTemplates = dict;
                }

                return _quotaTemplates;
            }
        }

        public async Task<OpResult<UserQuota>> ConsumeQuotaAsync(int quotaId, int amountConsumed)
        {
            var result = new OpResult<UserQuota>();

            try
            {
                // Part 1:  Update the quota.
                var toUpdate = await Ctx.UserQuotas.FindAsync(quotaId);
                if (toUpdate == null)
                {
                    result.AddError(Guid.NewGuid(), $"The UserQuota with Id {quotaId} was not found.");
                    return result;
                }
                else
                {
                    toUpdate.ConsumedBudget = toUpdate.ConsumedBudget + amountConsumed;
                    toUpdate.UpdatedDateUtc = DateTime.Now;

                    Ctx.UserQuotas.Update(toUpdate);
                    await Ctx.SaveChangesAsync();
                }

                // Part 2:  Return the updated Quotas.
                var updatedQuota = await LoadUserQuotaAsync(toUpdate.UserId);
                if (updatedQuota == null)
                {
                    result.AddError(Guid.NewGuid(), $"Unable to load the updated user quota for user {toUpdate.UserId}");
                }
                else
                {
                    result.Payload = updatedQuota.Payload;
                    if (updatedQuota.Errors.Any())
                    {
                        foreach (var error in updatedQuota.Errors)
                        {
                            result.AddError(error.Key, error.Value);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                string fatalError = $"An error occurred while updating quota id {quotaId}: {ex.Message}";
                result.AddError(Guid.NewGuid(), fatalError);
            }

            return result;
        }

        public async Task<OpResult<UserQuota>> LoadUserQuotaAsync(string userId)
        {
            UserQuota payload = new UserQuota();
            payload.UserId = userId;

            // Don't forget!!!  When assigning values to this payload, 
            // Get to the payload via the result.Payload.[property] route.
            // Assigning to payload.[property] won't make it into the result.
            OpResult<UserQuota> result = new OpResult<UserQuota>();

            try
            {
                var quotaData = await Ctx.UserQuotas.Where(q => q.UserId == userId).ToListAsync();
                var aiQuota = quotaData.ExtractQuotaOfKind(Templates[MeteredResourceKind.NpcAiDetail.ToString()]);
                var storageQuota = quotaData.ExtractQuotaOfKind(Templates[MeteredResourceKind.NpcStorage.ToString()]);

                if(aiQuota != null)
                {
                    payload.AiGenerations = aiQuota;
                }
                if (storageQuota != null)
                {
                    payload.Storage = storageQuota;
                }

                result.Payload = payload;
            }
            catch (Exception ex)
            {
                string message = $"Could not load the quotas for user id {userId}:  {ex.Message}";
                result.AddError(Guid.NewGuid(), message);
            }

            return result;
        }

        public async Task<OpResult<UserQuota>> ReleaseQuotaAsync(int quotaId, int amountReleased)
        {
            var result = new OpResult<UserQuota>();

            try
            {
                // Part 1:  Update the quota.
                var toUpdate = await Ctx.UserQuotas.FindAsync(quotaId);
                if (toUpdate == null)
                {
                    result.AddError(Guid.NewGuid(), $"The UserQuota with Id {quotaId} was not found.");
                    return result;
                }
                else
                {
                    toUpdate.ConsumedBudget = toUpdate.ConsumedBudget - amountReleased;
                    toUpdate.UpdatedDateUtc = DateTime.Now;

                    Ctx.UserQuotas.Update(toUpdate);
                    await Ctx.SaveChangesAsync();
                }

                // Part 2:  Return the updated Quotas.
                var updatedQuota = await LoadUserQuotaAsync(toUpdate.UserId);
                if (updatedQuota == null)
                {
                    result.AddError(Guid.NewGuid(), $"Unable to load the updated user quota for user {toUpdate.UserId}");
                }
                else
                {
                    result.Payload = updatedQuota.Payload;
                    if (updatedQuota.Errors.Any())
                    {
                        foreach (var error in updatedQuota.Errors)
                        {
                            result.AddError(error.Key, error.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string fatalError = $"An error occurred while updating quota id {quotaId}: {ex.Message}";
                result.AddError(Guid.NewGuid(), fatalError);
            }

            return result;
        }

        public async Task<OpResult> LogTokenConsumption(TokenConsumptionEntry meterEntry)
        {
            OpResult result = new OpResult();

            try
            {
                TokenConsumptionSqlModel row = meterEntry.ToRow();
                Ctx.TokenConsumptions.Add(row);
                await Ctx.SaveChangesAsync();
            }
            catch(Exception e)
            {
                Guid errId = Guid.NewGuid();
                _logger?.LogError(e, errId.ToString());
                result.AddError(errId, "An exception occurred while recording the token usage log entry.");
            }

            return result;
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

        
    }
}
