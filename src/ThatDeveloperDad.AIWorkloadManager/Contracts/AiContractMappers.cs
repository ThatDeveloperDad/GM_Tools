using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatDeveloperDad.LlmAccess.Contracts;

namespace ThatDeveloperDad.AIWorkloadManager.Contracts
{
	internal static class AiContractMappers
	{
		public static AiConsumption ToLocalModel(this AiUsage providerModel)
		{
			AiConsumption local = new AiConsumption
				(
					modelId: providerModel.ModelId,
					inputTokens: providerModel.PromptTokens,
					outputTokens: providerModel.ResponseTokens
				);

			return local;
		}
	}
}
