using GameTools.BlazorClient.Components.Pages.ComponentServices;
using GameTools.BlazorClient.Services;
using Microsoft.AspNetCore.Components;
using ThatDeveloperDad.Framework.Converters;

namespace GameTools.BlazorClient.Components.Pages.Partials
{
	public partial class ViewNpcDetails
	{
		public bool _isSaveButtonVisible;
        private bool _hasChanges;

		[CascadingParameter]
		protected AppStateProvider? AppContext { get; set; }

		[Parameter]
        public Func<NpcClientModel, Task>? SaveNpc_Clicked { get; set; }

        [Parameter]
        public NpcClientModel CurrentNpc { get; set; }

		[Parameter]
		public string Visibility { get; set; }

		[Parameter]
        public bool UseMetric { get; set; }

        [Parameter]
        public PageEventSink? NpcNotifier { get; set; }

        public ViewNpcDetails()
        {
            CurrentNpc = CurrentNpc ?? new NpcClientModel();
            Visibility = false.AsVisibility();
        }

		protected override async Task OnParametersSetAsync()
		{   
			_hasChanges = false;
			int npcId = CurrentNpc?.NpcModel?.Id ?? 0;
			if (npcId == 0)
			{
				_hasChanges = true;
			}
			await SetActionButtonStates();   
		}

		protected override async Task OnInitializedAsync()
		{
			await SetActionButtonStates();
		}

		private async Task SetActionButtonStates()
		{
			_isSaveButtonVisible = await ShouldShowSaveButton();
		}

		public string NpcName => CurrentNpc.NpcName;

        public string Species => CurrentNpc.Species;
        public string Gender => CurrentNpc.Gender;
        public string Pronouns => CurrentNpc.Pronouns;
        public int Age => CurrentNpc.Age;
        public string Height => CurrentNpc.Height;
        public string Weight => CurrentNpc.Weight;
        public string Appearance => CurrentNpc.Appearance;

        public string DetailedAppearance => CurrentNpc.GenAppearance;

        public string PersonalityDescription => CurrentNpc.PersonalityDescription;

        public string History => CurrentNpc.History;

        public string DetailedHistory => CurrentNpc.GenHistory;

        public string Profession => CurrentNpc.Profession;

        public string DetailedProfession => CurrentNpc.GenProfession;

        private async Task<bool> ShouldShowSaveButton()
        {
			if(SaveNpc_Clicked == null)
            {
                return false;
            }

            if(_hasChanges == false)
			{
				return false;
			}

            if(AppContext == null)
            {
                return false;
            }

            var userQuota = await AppContext.GetUserLimits();

            if(userQuota == null)
            {
                return false;
            }

            int availableStorageSlots = userQuota.NpcsInStorage.AvailableQuota;

            if(availableStorageSlots <= 0)
            {
                return false;
            }

            return true;
		}

		[CascadingParameter(Name = "LoadingOverlay")]
		protected ContentLoadingComponent? LoadingOverlay { get; set; }

		public async Task OnSaveNpcClicked()
        {
            if (LoadingOverlay != null)
            {
                await LoadingOverlay
                        .SetLoadingState(true, "Giving the NPC a home.");
            }

            if (SaveNpc_Clicked != null)
            {
                await SaveNpc_Clicked.Invoke(CurrentNpc);
            }

            if (LoadingOverlay != null)
            {
                await LoadingOverlay.SetLoadingState(false);
            }
        }
    }
}
