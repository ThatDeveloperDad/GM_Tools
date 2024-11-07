using GameTools.Framework.Concepts;
using GameTools.MeteredUsageAccess.ResourceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableStorageTest.LocalServices
{
	internal class Entitlements
	{

        public Entitlements()
        {
			UserId = string.Empty;

			Storage = new Quota()
			{
				QuotaId = 1,
				MeteredResource = MeteredResourceKind.NpcStorage
			};

			NpcAiGeneration = new Quota()
			{
				QuotaId = 2,
				MeteredResource = MeteredResourceKind.NpcAiDetail
			};
        }

        public string UserId { get; set; }

		public Quota? GetQuota(int quotaId)
		{
			// This is a little gross, but is a holdover from
			// when Quotas were stored relationally instead of
			// as complex properties in Table Storage.
			if (Storage.QuotaId == quotaId)
			{
				return Storage;
			}
			else if (NpcAiGeneration.QuotaId == quotaId)
			{
				return NpcAiGeneration;
			}
			else
			{
				return null;
			}
		}

		public Quota Storage { get; set; }

		public Quota NpcAiGeneration { get; set; }
	}
}
