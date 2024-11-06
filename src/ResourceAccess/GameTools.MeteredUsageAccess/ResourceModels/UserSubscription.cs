using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.ResourceModels
{
    public class UserSubscription
    {
        public UserSubscription()
        {
            UserId = string.Empty;
            Quotas = new UserQuota();
        }

        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime StartDateUtc { get; set; }

        public DateTime EndDateUtc { get; set; }

        public bool WillRenew { get; set; }

        public UserQuota Quotas { get; set; }
    }
}
