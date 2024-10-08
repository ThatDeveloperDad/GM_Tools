using GameTools.BlazorClient.Services;
using GameTools.Framework.Converters;
using Microsoft.AspNetCore.Components;

namespace GameTools.BlazorClient.Components.Pages.Partials
{
	public partial class UsageInformation
	{
		private UserLimitsModel? _userLimitsModel;

		protected override Task OnInitializedAsync()
		{
			return base.OnInitializedAsync();
		}

		protected override async Task OnParametersSetAsync()
		{
			if(AppContext != null)
			{
				AppContext.ContextItemChanged += OnContextItemChangedAsync;
			}

			if(AppContext?.GetUserLimits() == null && AppContext != null)
			{
				_userLimitsModel = await AppContext.GetUserLimits();
				StateHasChanged();
			}
			
		}

		public bool IsReady => (_userLimitsModel != null);

		[CascadingParameter]
		protected AppStateProvider? AppContext { get; set; }

		public string? StorageQuotaString => BuildQuotaString(_userLimitsModel?.NpcsInStorage);

		public string? AiGenQuotaString => BuildQuotaString(_userLimitsModel?.NpcAiDescriptions);

		private string BuildQuotaString(LimitedResourceModel? quota)
		{
			string quotaString = string.Empty;

			if(quota!=null)
			{
				string resourceLabel = quota.ResourceTitle.ToUiLabel();
				quotaString = $"{resourceLabel} {quota.ConsumedUnits} / {quota.UnitBudget}";
			}

			return quotaString;
		}

		private async Task OnContextItemChangedAsync(string contextKey)
		{
			if(contextKey == nameof(UserLimitsModel))
			{
				_userLimitsModel = await AppContext!.GetUserLimits();
				StateHasChanged();
			}
		}
    }
}
