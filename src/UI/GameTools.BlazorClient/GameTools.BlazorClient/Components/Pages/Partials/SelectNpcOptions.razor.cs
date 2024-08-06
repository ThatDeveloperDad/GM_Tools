using GameTools.TownsfolkManager.Contracts;
using Microsoft.AspNetCore.Components;

namespace GameTools.BlazorClient.Components.Pages.Partials
{
	public partial class SelectNpcOptions
	{
		
		[Parameter]
		public Dictionary<string, string[]> SelectableOptions { get; set; }

		[Parameter]
		public Dictionary<string, string?> SelectedOptions { get; set; }	

        public SelectNpcOptions()
        {
            SelectableOptions = SelectableOptions?? new Dictionary<string, string[]>();
			SelectedOptions = SelectedOptions ?? new Dictionary<string, string?>();
        }

        public string? Species 
		{
			get
			{
				return SelectedOptions[nameof(Townsperson.Species)] ?? string.Empty;
			}
			set
			{	
				SelectedOptions[nameof(Townsperson.Species)] = value;
			}
		}

        public string? Background 
		{ 
			get
			{
				return SelectedOptions[nameof(Townsperson.Background)] ?? string.Empty;
			}
			set
			{
				SelectedOptions[nameof(Townsperson.Background)] = value;
			}
		}

		public string? Vocation
		{
			get
			{
				return SelectedOptions[nameof(Townsperson.Vocation)] ?? string.Empty;
			}
			set
			{
				SelectedOptions[nameof(Townsperson.Vocation)] = value;
			}
		}

		
    }
}
