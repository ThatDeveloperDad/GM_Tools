using Microsoft.AspNetCore.Components;
using GameTools.BlazorClient.Services;
using GameTools.TownsfolkManager.Contracts;
namespace GameTools.BlazorClient.Components.Pages.PageModels
{
	public class CreateNpcPageModel:ComponentBase
	{

        public CreateNpcPageModel():base()
        {
			
            CurrentNpc = new NpcClientModel();
			SelectableNpcOptions = new Dictionary<string, string[]>();
			SelectedNpcOptions = new Dictionary<string, string?>();
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

		public void OnRerollClick()
		{
			CurrentNpc = GenerateNpc();
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

			if(SelectedNpcOptions.Any())
			{
				// Any selected options with an empty string for the value must be set null.
				foreach (string optionKey in SelectedNpcOptions.Keys)
				{
					string? optionValue = SelectedNpcOptions[optionKey];
					if (optionValue != null & optionValue == string.Empty)
					{
						SelectedNpcOptions[optionKey] = null;
					}
				}
				model = NpcServices!.GenerateRandomNPC(SelectedNpcOptions);
			}
			else
			{
				model = NpcServices!.GenerateRandomNPC();
			}
			
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
