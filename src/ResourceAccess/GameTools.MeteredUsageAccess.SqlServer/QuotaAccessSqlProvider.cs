using GameTools.MeteredUsageAccess;
using GameTools.MeteredUsageAccess.ResourceModels;
using GameTools.MeteredUsageAccess.SqlServer.Context;
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

        public Task<OpResult<UserQuota>> ConsumeQuotaAsync(int quotaId, int amountConsumed)
        {
            throw new NotImplementedException();
        }

        public Task<OpResult<UserQuota>> LoadUserQuotaAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<OpResult<UserQuota>> ReleaseQuotaAsync(int quotaId, int amounReleased)
        {
            throw new NotImplementedException();
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
