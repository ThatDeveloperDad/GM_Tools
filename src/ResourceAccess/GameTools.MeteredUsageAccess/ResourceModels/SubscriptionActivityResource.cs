using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.ResourceModels
{
	public class SubscriptionActivityResource
	{
        public string ActivityKind { get; set; }

        public DateTime ActivityDateUTC { get; set; }
    }
}
