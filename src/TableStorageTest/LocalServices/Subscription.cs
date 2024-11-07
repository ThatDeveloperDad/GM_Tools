using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableStorageTest.LocalServices
{
	internal class Subscription
	{
        public Subscription()
        {
            UserQuotas = new Entitlements();
        }

		public string CurrentStatus { get; set; } = "None";

        public string UserId { get; set; } = string.Empty;

		public DateTime StartDateUtc { get; set; }

		public DateTime EndDateUtc { get; set; }

		public bool WillRenew { get; set; }

		public Entitlements UserQuotas { get; set; }
	}
}
