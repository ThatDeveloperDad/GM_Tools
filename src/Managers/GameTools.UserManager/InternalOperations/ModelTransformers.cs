using GameTools.MeteredUsageAccess.ResourceModels;
using GameTools.UserManager.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.UserManager.InternalOperations
{
	internal static class ModelTransformers
	{
		public static QuotaContainer? ToAppModel(this UserQuota? storageModel)
		{
			QuotaContainer? appModel = null;

			if (storageModel != null)
			{
				appModel = new QuotaContainer()
				{
					UserId = storageModel.UserId,
					NpcAiGenerations = storageModel.AiGenerations.ToAppModel(),
					NpcStorage = storageModel.Storage.ToAppModel()
				};
			}

			return appModel;
		}

		public static Quota? ToAppModel(this ResourceQuota? storageModel)
		{
			Quota? appModel = null;

			if (storageModel != null)
			{
				appModel = new Quota()
				{
					MeteredResource = storageModel.MeteredResource,
					Budget = storageModel.Budget,
					Consumption = storageModel.Consumption
				};
			}

			return appModel;
		}
	}
}
