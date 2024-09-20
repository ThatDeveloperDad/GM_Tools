using GameTools.Framework.Concepts;

namespace GameTools.BlazorClient.Services
{
	public class UserLimitsModel
	{
		
		public UserLimitsModel() 
		{ 
			UserId = string.Empty;

			NpcAiDescriptions = new LimitedResourceModel(MeteredResourceKind.NpcAiDetail);
			NpcsInStorage = new LimitedResourceModel(MeteredResourceKind.NpcStorage);
		}

		public string UserId { get; init; }

		public LimitedResourceModel NpcAiDescriptions { get; init; }

		public LimitedResourceModel NpcsInStorage { get; init; }
	}
}
