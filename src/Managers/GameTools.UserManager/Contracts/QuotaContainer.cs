using GameTools.Framework.Concepts;
using GameTools.MeteredUsageAccess.ResourceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.UserManager.Contracts
{
	public class QuotaContainer
	{

		public QuotaContainer() 
		{
			UserId = string.Empty;
			NpcStorage = new Quota()
			{
				MeteredResource = MeteredResourceKind.NpcStorage
			};

			NpcAiGenerations = new Quota()
			{
				MeteredResource = MeteredResourceKind.NpcAiDetail
			};
		}

		public string UserId { get; set; }

		public Quota? NpcStorage { get; set; }

		public Quota? NpcAiGenerations { get; set; }


	}
}
