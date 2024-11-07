using GameTools.MeteredUsageAccess.ResourceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableStorageTest.LocalServices
{
	internal class UserAccount
	{
		public string UserId { get; set; } = string.Empty;

		public string DisplayName { get; set; } = string.Empty;

		public string SubscriptionStatus { get; set; } = "None";

		public List<UserVendorId> ExternalIds { get; set; } = new List<UserVendorId>();

		public Subscription? Subscription { get; set; }
    }
}
