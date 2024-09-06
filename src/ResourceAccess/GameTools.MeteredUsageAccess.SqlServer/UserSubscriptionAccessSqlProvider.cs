using GameTools.MeteredUsageAccess.ResourceModels;
using GameTools.MeteredUsageAccess.SqlServer.Context;
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
        private readonly string _cn;
        private readonly ILogger<UserSubscriptionAccessSqlProvider>? _logger;
        private UsageAccessDbContext? _ctx;
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

        public Task<OpResult<UserSubscription>> CancelSubscription(string userId, bool cancelImmediately = false)
        {
            throw new NotImplementedException();
        }

        public Task<OpResult<UserSubscription>> LoadSubscription(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<OpResult<UserSubscription>> RenewSubscription(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<OpResult<UserSubscription>> StartSubscription(string userId, string? subscriptionKind = null)
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
