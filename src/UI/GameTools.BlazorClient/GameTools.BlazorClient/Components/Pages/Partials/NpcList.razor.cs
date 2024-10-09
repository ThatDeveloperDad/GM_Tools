using GameTools.BlazorClient.Components.Pages.ComponentServices;
using GameTools.BlazorClient.Components.Pages.PageModels;
using GameTools.BlazorClient.Services;
using GameTools.Framework.Contexts;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using ThatDeveloperDad.Framework.Converters;

namespace GameTools.BlazorClient.Components.Pages.Partials
{
    public partial class NpcList
    {
        [Inject]
        public ILogger<NpcList>? Logger { get; set; }

        [Parameter]
        public Action? NewNpcHandler { get; set; }

        [Parameter]
        public Func<int, Task>? ViewNpcHandler { get; set; }

        private CreateNpcPageStates _currentPageState;

        [Parameter]
        public CreateNpcPageStates CurrentPageState { get; set; }

        [Parameter]
        public string Visibility { get; set; }

        [Parameter]
        public PageEventSink? NpcNotifier { get; set; }

        [Inject]
        protected NpcServiceProxy? NpcServices { get; set; }

        [CascadingParameter]
        protected AppStateProvider? AppContext { get; set; }

		[CascadingParameter(Name ="LoadingOverlay")]
		protected ContentLoadingComponent? LoadingOverlay { get; set; }

		public NpcList()
		{
			FilteredNpcs = Array.Empty<NpcFilterRowModel>();
			EmptyTableText = "Loading ...";
            Visibility = false.AsVisibility();
		}

		protected override void OnInitialized()
        {
            base.OnInitialized();
            RegisterNpcEventHandlers();
        }

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await ExecuteFilter();
                StateHasChanged();
            }
		}

        public async Task OnPageStateChangingAsync(CreateNpcPageStates newState)
        {
            if(newState == CreateNpcPageStates.List)
            {
                await ExecuteFilter();
            }
        }

        public string EmptyTableText { get; private set; }

        public NpcFilterRowModel[] FilteredNpcs { get; set; }

        public bool ShowCreateButton => NewNpcHandler != null;

        public bool ShowViewNpcButton => ViewNpcHandler != null;

        public void OnNewNpcClicked()
        {
            NewNpcHandler?.Invoke();
        }

        public async Task OnViewNpcClicked(int npcId)
        {
            if(ViewNpcHandler !=null)
            {
                await ViewNpcHandler.Invoke(npcId);
            }
            
        }

        private void RegisterNpcEventHandlers()
        {
            NpcNotifier?.RegisterEventListener(HandlePageStateChanged);
        }


        private async Task HandlePageStateChanged(PageStateChangedEvent notification)
        {
            _currentPageState = notification.NewValue;

            if(_currentPageState == CreateNpcPageStates.List)
            {
                await ExecuteFilter();
                StateHasChanged();
            }
        }

        private async Task ExecuteFilter()
        {
            
            // If we're not looking at the NPC List, don't fetch.
            if(_currentPageState != CreateNpcPageStates.List)
            {
                return;
            }

            GuardNpcServicesExists();
            GuardUserExists();

			GameToolsUser user = AppContext?.GetCurrentUser(true)!;

			NpcClientFilter filter = new NpcClientFilter();
            filter.UserId = user.UserId;

            await ShowLoadingMessage("Loading NPCs...");
            var proxyResult = await NpcServices!.FilterTownsfolk(filter);
            await HideLoadingMessage();
            

            if (proxyResult == null)
            {
                throw new Exception("Something awful happened.");
            }
            else
            {
                if (proxyResult.WasSuccessful)
                {
                    FilteredNpcs = proxyResult.Payload?.ToArray()
                        ?? Array.Empty<NpcFilterRowModel>();

                    if(FilteredNpcs.Length == 0)
                    {
                        EmptyTableText = "No Characters Found.";
					}

                   
                }
                else
                {
                    //Handle any errors that came back.
                    var sb = new StringBuilder();
                    foreach(var kvp in proxyResult.Errors)
                    {
                        sb.AppendLine(kvp.Value);
                    }

                   

                    throw new Exception($"Fetching the NPC list failed.  {Environment.NewLine}{sb.ToString()}");
                }
            }
        }

        private async Task ShowLoadingMessage(string text)
        {
            if(LoadingOverlay != null)
            {
                await LoadingOverlay.SetLoadingState(true, text);
            }
        }

        private async Task HideLoadingMessage()
        {
            if(LoadingOverlay!=null)
            {
                await LoadingOverlay.SetLoadingState(false);
            }
        }

		private void GuardNpcServicesExists(
			[CallerMemberName] string callerMethod = "",
			[CallerFilePath] string callerFile = "",
			[CallerLineNumber] int callerLineNum = 0)
		{
			if (NpcServices == null)
			{
				Guid errorId = Guid.NewGuid();
				string message = $"Cannot perform the requested operation.  NpcServices is null.  ErrorId: {errorId}";

				LogError(callerMethod, callerFile, callerLineNum, errorId, message);

				throw new InvalidOperationException(message);
			}
		}

		private void GuardAppContextExists(
			[CallerMemberName] string callerMethod = "",
			[CallerFilePath] string callerFile = "",
			[CallerLineNumber] int callerLineNum = 0)
		{
			if (AppContext == null)
			{
				Guid errorId = Guid.NewGuid();
				string message = $"Cannot perform the requested task while the Application Context is not initialized.  ErrorId: {errorId}";

				LogError(callerMethod, callerFile, callerLineNum, errorId, message);

				throw new InvalidOperationException(message);
			}
		}

		private void GuardUserExists(
			[CallerMemberName] string callerMethod = "",
			[CallerFilePath] string callerFile = "",
			[CallerLineNumber] int callerLineNum = 0)
		{
			GuardAppContextExists();
			var user = AppContext!.GetCurrentUser(true);
			if (user == null)
			{
				Guid errorId = Guid.NewGuid();
				string message = $"Cannot perform the requested task while the CurrentUser is unknown.  ErrorId: {errorId}";

				LogError(callerMethod, callerFile, callerLineNum, errorId, message);

				throw new InvalidOperationException(message);
			}
		}

		private void LogError(string callerMethod, string callerFile, int callerLineNum, Guid errorId, string message)
		{
			var logEntry = new
			{
				ErrorId = errorId.ToString(),
				Message = message,
				CallerMethod = callerMethod,
				CallerFile = callerFile,
				CallerLineNum = callerLineNum,
				ErrorTime = DateTime.UtcNow.ToString()
			};

			string logEntryJson = JsonSerializer.Serialize(logEntry);
			Logger?.LogError(logEntryJson);
		}
	}
}
