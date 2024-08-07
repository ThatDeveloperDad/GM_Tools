using GameTools.BlazorClient.Services;
using GameTools.TownsfolkManager.Contracts;
using Microsoft.AspNetCore.Components;
using ThatDeveloperDad.Framework.Converters;

namespace GameTools.BlazorClient.Components.Pages.Partials
{
	public partial class SelectNpcOptions
	{

		[Parameter]
		public Action? GenerateNpc_Clicked { get; set; }

		[Parameter]
		public Dictionary<string, string[]> SelectableOptions { get; set; }

		[Parameter]
		public NpcUserOptions UserOptions { get; set; }

		public bool ShowOptionsPanel { get; set; }

		public SelectNpcOptions()
        {
            SelectableOptions = SelectableOptions?? new Dictionary<string, string[]>();
			UserOptions = UserOptions ?? new NpcUserOptions();
        }

		protected override void OnInitialized()
		{
			base.OnInitialized();
			SetOptionPanelVisibility(false);
		}

		public string? Species
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

		public string? Background 
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

		public string? Vocation
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

		public bool IsRerollVisible => GenerateNpc_Clicked != null;

		public string ToggleText { get; private set; }

		public void ToggleOptionPanel()
		{
			bool newState = !ShowOptionsPanel;
			SetOptionPanelVisibility(newState);
		}

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
    }
}
