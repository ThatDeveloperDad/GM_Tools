using GameTools.MeteredUsageAccess.ResourceModels;
using GameTools.MeteredUsageAccess.SqlServer.Context.SqlModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.SqlServer.Transformers
{
    internal static class SubscriptionDataTransformers
    {
        public static UserSubscription ToDto(this UserSubscriptionSqlModel poco)
        {
            UserSubscription dto = new UserSubscription()
            {
                Id = poco.Id,
                UserId = poco.UserId,
                StartDateUtc = poco.PeriodStartUtc,
                EndDateUtc = poco.PeriodEndUtc,
                WillRenew = (poco.UnsubscribedDateUtc.HasValue == false)
            };

            return dto;
        }
    }
}
