using GameTools.BlazorClient.Components.Pages.ComponentServices;
using GameTools.BlazorClient.Components.Pages.PageModels;
using GameTools.BlazorClient.Services;
using GameTools.Framework.Contexts;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace GameTools.BlazorClient.Components.Pages.Partials
{
    public partial class NpcList
    {
        [Parameter]
        public Action? NewNpcHandler { get; set; }

        [Parameter]
        public Func<int, Task> ViewNpcHandler { get; set; }

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
        protected AppStateProvider AppContext { get; set; }

		[CascadingParameter(Name ="LoadingOverlay")]
		protected ContentLoadingComponent? LoadingOverlay { get; set; }

		public NpcList()
		{
			FilteredNpcs = Array.Empty<NpcFilterRowModel>();
			EmptyTableText = "Loading ...";
            
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

            var user = AppContext.GetCurrentUser();

            if(user == null)
            {
                return;
            }

            GuardNpcServicesExist();


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

        private void GuardNpcServicesExist()
        {
            if (NpcServices == null)
            {
                throw new InvalidOperationException("Cannot execute the requested operation because the ServiceProxy is null.");
            }
        }
    }
}
