using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.ResourceModels
{
	public class UserResource
	{
        public UserResource()
        {
			UserId = string.Empty;
			DisplayName = string.Empty;
			Ids = new List<UserIdResource>();
			SubscriptionActivity = new List<SubscriptionActivityResource>();
        }

        public string UserId { get; set; }

		public string DisplayName { get; set; }

		public List<UserIdResource> Ids { get; set; }

		public UserSubscription? CurrentSubscription { get; set; }

		public List<SubscriptionActivityResource> SubscriptionActivity { get; set; }
	}
}
