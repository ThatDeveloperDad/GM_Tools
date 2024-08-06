using GameTools.BlazorClient.Services;
using GameTools.TownsfolkManager.Contracts;
using Microsoft.AspNetCore.Components;

namespace GameTools.BlazorClient.Components.Pages.Partials
{
	public partial class SelectNpcOptions
	{
		
		[Parameter]
		public Dictionary<string, string[]> SelectableOptions { get; set; }

		[Parameter]
		public NpcUserOptions UserOptions { get; set; }

        public SelectNpcOptions()
        {
            SelectableOptions = SelectableOptions?? new Dictionary<string, string[]>();
			UserOptions = UserOptions ?? new NpcUserOptions();
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
    }
}
