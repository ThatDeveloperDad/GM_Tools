﻿using GameTools.BlazorClient.Components.Pages.ComponentServices;
using GameTools.BlazorClient.Services;
using Microsoft.AspNetCore.Components;

namespace GameTools.BlazorClient.Components.Pages.Partials
{
	public partial class ViewNpcAttributes
    {
        [Parameter]
        public Func<NpcClientModel, Task>? GenerateAi_Clicked { get; set; }

        [Parameter]
        public Func<NpcClientModel, Task>? SaveNpc_Clicked { get; set; }

		[Parameter]
		public Action? GenerateNpc_Clicked { get; set; }

		[Parameter]
        public NpcClientModel CurrentNpc { get; set; }

		[Parameter]
		public Dictionary<string, string[]> SelectableOptions { get; set; }

		[Parameter]
		public NpcUserOptions UserOptions { get; set; }

		public bool ShowOptionsPanel { get; set; }

		[Parameter]
		public string Visibility { get; set; }

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
			SelectableOptions = SelectableOptions ?? new Dictionary<string, string[]>();
			UserOptions = UserOptions ?? new NpcUserOptions();
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			SetOptionPanelVisibility(false);
		}

		[CascadingParameter(Name = "LoadingOverlay")]
		protected ContentLoadingComponent? LoadingOverlay { get; set; }

		public string? SelectedSpecies
        {
            get
            {
                return UserOptions.Species;
            }
            set
            {
                UserOptions.Species = value;
            }
        }

        public string? SelectedBackground
        {
            get
            {
                return UserOptions.Background;
            }
            set
            {
                UserOptions.Background = value;
            }
        }

        public string? SelectedVocation
        {
            get
            {
                return UserOptions.Vocation;
            }
            set
            {
                UserOptions.Vocation = value;
            }
        }

        public bool? IsRetired 
        {  
            get
            {
                return UserOptions.IsRetired;
            }
            set
            {
                UserOptions.IsRetired = value;
            }
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

		public bool IsRerollVisible => GenerateNpc_Clicked != null;

        public void ToggleOptionPanel()
        {
            bool newState = !ShowOptionsPanel;
            SetOptionPanelVisibility(newState);
            StateHasChanged();
        }

        public string ToggleText { get; private set; }

		private void SetOptionPanelVisibility(bool shouldShow)
		{
			ToggleText = (shouldShow) ? "Hide" : "Show";
			ShowOptionsPanel = shouldShow;
		}

		public void OnCreateNpcClick()
		{
			this.GenerateNpc_Clicked?.Invoke();
			SetOptionPanelVisibility(false);
		}

		public bool AiButtonIsHandled => GenerateAi_Clicked != null;

        public async Task OnAiButtonClick()
        {
            await LoadingOverlay.SetLoadingState(true, "Thinking...");
            await GenerateAi_Clicked?.Invoke(CurrentNpc);
            await LoadingOverlay.SetLoadingState(false);
		}

        public bool SaveButtonIsHandled => SaveNpc_Clicked != null;

        public async void OnSaveButtonClick()
        {
            await LoadingOverlay.SetLoadingState(true, "Saving...");
            SaveNpc_Clicked?.Invoke(CurrentNpc);
			await LoadingOverlay.SetLoadingState(false);
		}

        public void UpdateClientModel(NpcClientModel model)
        {
            CurrentNpc = model;
            StateHasChanged();
        }
    }
}
