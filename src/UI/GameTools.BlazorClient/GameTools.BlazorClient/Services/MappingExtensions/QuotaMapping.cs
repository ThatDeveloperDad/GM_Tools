using GameTools.UserManager.Contracts;

namespace GameTools.BlazorClient.Services.MappingExtensions
{
	internal static class QuotaMapping
	{
		public static UserLimitsModel ToUiModel(this QuotaContainer apiModel)
		{
			UserLimitsModel uiModel = new UserLimitsModel()
			{
				UserId = apiModel.UserId
			};
			uiModel.NpcsInStorage.SetValues(
				apiModel!.NpcStorage!.QuotaId,
				apiModel.NpcStorage.Budget,
				apiModel.NpcStorage.Consumption);

			uiModel.NpcAiDescriptions.SetValues(
				apiModel!.NpcAiGenerations!.QuotaId,
				apiModel.NpcAiGenerations.Budget,
				apiModel.NpcAiGenerations.Consumption);

			return uiModel;
		}
	}
}
