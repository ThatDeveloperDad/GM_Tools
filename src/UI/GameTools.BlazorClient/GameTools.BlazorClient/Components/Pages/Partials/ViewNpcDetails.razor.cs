using GameTools.BlazorClient.Components.Pages.ComponentServices;
using GameTools.BlazorClient.Services;
using Microsoft.AspNetCore.Components;

namespace GameTools.BlazorClient.Components.Pages.Partials
{
	public partial class ViewNpcDetails
	{

        [Parameter]
        public Func<NpcClientModel, Task>? SaveNpc_Clicked { get; set; }

        [Parameter]
        public Action ShowList_Clicked { get; set; }

        [Parameter]
        public NpcClientModel CurrentNpc { get; set; }

        [Parameter]
        public bool UseMetric { get; set; }

        [Parameter]
        public PageEventSink? NpcNotifier { get; set; }

        public ViewNpcDetails()
        {
            CurrentNpc = CurrentNpc ?? new NpcClientModel();
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

        public bool IsSaveButtonVisible => SaveNpc_Clicked != null;

        public void OnSaveNpcClicked()
        {
            SaveNpc_Clicked?.Invoke(CurrentNpc);
        }

        public bool IsListButtonVisible => ShowList_Clicked != null;

        public void OnShowListClicked()
        {
            ShowList_Clicked?.Invoke();
        }
    }
}
