using Microsoft.AspNetCore.Components;
using GameTools.BlazorClient.Services;
using GameTools.TownsfolkManager.Contracts;
namespace GameTools.BlazorClient.Components.Pages.PageModels
{
	public class CreateNpcPageModel:ComponentBase
	{
		protected override void OnInitialized()
		{
			base.OnInitialized();
			CurrentNpc = GenerateNpc();

			
		}

		[Inject]
		protected NpcServiceProvider NpcServices { get; set; }

		public NpcClientModel CurrentNpc { get; private set; }

		public void OnRerollClick()
		{
			CurrentNpc = GenerateNpc();
		}

		public async Task OnAiDescriptionRequested(NpcClientModel npcModel) 
		{
			await GetAiDescription(npcModel);
		}

		private NpcClientModel GenerateNpc()
		{
			NpcClientModel model = NpcServices.GenerateRandomNPC();
			return model;
		}

		private async Task<NpcClientModel> GetAiDescription(NpcClientModel npcModel)
		{
			NpcClientModel npcAi = await NpcServices.GetAiDescription(npcModel);
			return npcAi;
		}
	}
}
