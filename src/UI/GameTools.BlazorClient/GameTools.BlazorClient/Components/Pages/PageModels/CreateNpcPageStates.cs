namespace GameTools.BlazorClient.Components.Pages.PageModels
{
	public enum CreateNpcPageStates
	{
		/// <summary>
		/// This page state means we're still setting up the attributes of the NPC
		/// and we have not yet generated or added any details.
		/// </summary>
		Preview,

		/// <summary>
		/// This page state is used when we've accepted the Random or Chosen attributes,
		/// and we're showing the generated or user supplied details.
		/// </summary>
		Details
	}
}
