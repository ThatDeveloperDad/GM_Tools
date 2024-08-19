using GameTools.BlazorClient.Components.Pages.ComponentServices;
using GameTools.BlazorClient.Services;
using Microsoft.AspNetCore.Components;

namespace GameTools.BlazorClient.Components.Pages.Partials
{
	public partial class ViewNpcAttributes
    {
        [Parameter]
        public Func<NpcClientModel, Task>? GenerateAi_Clicked { get; set; }

        

        [Parameter]
        public NpcClientModel CurrentNpc { get; set; }

        [Parameter]
        public bool UseMetric { get; set; }

        [Parameter]
        public PageEventSink? NpcNotifier { get; set; }

        /// <summary>
        /// Handles displaying the Townsperson in the ViewNpcAttributes view.
        /// </summary>
        /// <param name="model">The Townsperson instance to be displayed</param>
        /// <param name="useMetric">Optional.  Pass true to display NPC measurements in metric instead of imperial units.</param>
        public ViewNpcAttributes()
        {
            CurrentNpc = CurrentNpc??new NpcClientModel();
            UseMetric = false;
        }

        public string Species => CurrentNpc.Species;
        public string Gender => CurrentNpc.Gender;
        public string Pronouns => CurrentNpc.Pronouns;
        public int Age => CurrentNpc.Age;
        public string Height => CurrentNpc.Height;
        public string Weight => CurrentNpc.Weight;
        public string Appearance => CurrentNpc.Appearance;
        public string History => CurrentNpc.History;
        public string Profession => CurrentNpc.Profession;

        public bool AiButtonIsHandled => GenerateAi_Clicked != null;

        public void OnAiButtonClick()
        {
            GenerateAi_Clicked?.Invoke(CurrentNpc);
        }

        public void UpdateClientModel(NpcClientModel model)
        {
            CurrentNpc = model;
            StateHasChanged();
        }
    }
}
