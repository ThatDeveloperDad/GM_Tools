using GameTools.BlazorClient.Services;
using Microsoft.AspNetCore.Components;

namespace GameTools.BlazorClient.Components.Pages.Partials
{
	public partial class ViewNpcDetails
	{

        [Parameter]
        public NpcClientModel CurrentNpc { get; set; }

        [Parameter]
        public bool UseMetric { get; set; }

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
    }
}
