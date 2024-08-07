using GameTools.BlazorClient.Services;
using Microsoft.AspNetCore.Components;
namespace GameTools.BlazorClient.Components.Pages.PageModels
{
	public class CreateNpcPageModel:ComponentBase
	{

        public CreateNpcPageModel():base()
        {
			
            CurrentNpc = new NpcClientModel();
			SelectableNpcOptions = new Dictionary<string, string[]>();
			SelectedNpcOptions = new Dictionary<string, string?>();
			UserOptions = new NpcUserOptions();
        }

        protected override void OnInitialized()
		{
			base.OnInitialized();
			CurrentNpc = GenerateNpc();
			SelectableNpcOptions = LoadSelectableNpcOptions();
			foreach(string optionKey in SelectableNpcOptions.Keys)
			{
				SelectedNpcOptions[optionKey] = null;
			}
		}

		[Inject]
		protected NpcServiceProvider? NpcServices { get; set; }

		public NpcClientModel CurrentNpc { get; private set; }

		public Dictionary<string, string[]> SelectableNpcOptions { get; private set; }

		public Dictionary<string, string?> SelectedNpcOptions { get; private set; }

		public NpcUserOptions UserOptions { get; private set; }

		public void OnRerollClick()
		{
			CurrentNpc = GenerateNpc();
			StateHasChanged();
		}

		public async Task OnAiDescriptionRequested(NpcClientModel npcModel) 
		{
			await GetAiDescription(npcModel);
		}

		private Dictionary<string, string[]> LoadSelectableNpcOptions()
		{
			GuardNpcServicesExists();

			Dictionary<string, string[]>? options = NpcServices!.GetNpcOptions();
			
			return options;
		}

		private NpcClientModel GenerateNpc()
		{
			GuardNpcServicesExists();
			NpcClientModel model;

			model = NpcServices!.GenerateRandomNPC(UserOptions);
			return model;
		}

		private async Task<NpcClientModel> GetAiDescription(NpcClientModel npcModel)
		{
			//TODO:  This Guard => Null Forgiving on dependencies feels bad, man.
			// I won't even remember to do this stuff later until the compiler barks at me.
			// Think of a more elegant  way for components to hold and resolve their own dependencies.
			GuardNpcServicesExists();
			NpcClientModel npcAi = await NpcServices!.GetAiDescription(npcModel);
			return npcAi;
		}



		private void GuardNpcServicesExists()
		{
			if(NpcServices == null)
			{
				//TODO:  add logging.  Generate a Guid to identify the exception incident and attach that to the log
				// event, and the error message.
				throw new InvalidOperationException("Cannot complete the request due to an internal error.");
			}
		}
	}
}
