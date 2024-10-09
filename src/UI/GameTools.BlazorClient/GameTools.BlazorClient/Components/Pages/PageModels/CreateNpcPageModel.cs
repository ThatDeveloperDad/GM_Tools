using GameTools.BlazorClient.Components.Pages.ComponentServices;
using GameTools.BlazorClient.Services;
using GameTools.Framework.Contexts;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using ThatDeveloperDad.Framework.Converters;
using ThatDeveloperDad.Framework.Wrappers;
namespace GameTools.BlazorClient.Components.Pages.PageModels
{
	public class CreateNpcPageModel:ComponentBase
	{
		[Inject]
		public ILogger<CreateNpcPageModel>? Logger { get; set; }

        public CreateNpcPageModel():base()
        {
			NpcEventNotifier = new PageEventSink();

            PageState = CreateNpcPageStates.List;

            CurrentNpc = new NpcClientModel();
			SelectableNpcOptions = new Dictionary<string, string[]>();
			SelectedNpcOptions = new Dictionary<string, string?>();
			UserOptions = new NpcUserOptions();

			
        }

        protected override void OnInitialized()
		{
			base.OnInitialized();
			RegisterNpcEventHandlers();

			CurrentNpc = GenerateNpc();
			SelectableNpcOptions = LoadSelectableNpcOptions();
			foreach(string optionKey in SelectableNpcOptions.Keys)
			{
				SelectedNpcOptions[optionKey] = null;
			}
		}

		protected override async Task OnParametersSetAsync()
		{
			await base.OnParametersSetAsync();
			var user = AppContext?.GetCurrentUser(true);

			if(user != null)
			{
				CurrentUser = user;
			}

			LoadingOverlay = LoadingOverlay ?? new ContentLoadingComponent();
		}

		public PageEventSink NpcEventNotifier { get; set; }

        [Inject]
		protected NpcServiceProxy? NpcServices { get; set; }

		[CascadingParameter]
		protected AppStateProvider? AppContext { get; set; }

		[CascadingParameter(Name = "LoadingOverlay")]
		protected ContentLoadingComponent? LoadingOverlay { get; set; }

		public GameToolsUser? CurrentUser { get; set; }

		public CreateNpcPageStates PageState { get; set; }

		public NpcClientModel CurrentNpc { get; private set; }

		public Dictionary<string, string[]> SelectableNpcOptions { get; private set; }

		public Dictionary<string, string?> SelectedNpcOptions { get; private set; }

		public NpcUserOptions UserOptions { get; private set; }

		public void OnRerollClicked()
		{
			CurrentNpc = GenerateNpc();
			StateHasChanged();
		}

		public void OnStartOverClicked()
		{
			PageState = CreateNpcPageStates.Preview;
			UserOptions = new NpcUserOptions();
			CurrentNpc = GenerateNpc();
			StateHasChanged();
		}

		public async Task OnAiDescriptionRequested(NpcClientModel npcModel) 
		{
            
			//StateHasChanged();
            CurrentNpc = await GetAiDescription(npcModel);
			PageState = CreateNpcPageStates.Details;
			StateHasChanged();
		}

		public async Task OnSaveNpc(NpcClientModel npcModel)
		{
			PageState = CreateNpcPageStates.Details;
			StateHasChanged();
			CurrentNpc = await SaveNpc(npcModel);
			StateHasChanged();
		}

		public void OnCreateNewClicked()
		{
			PageState = CreateNpcPageStates.Preview;
			UserOptions = new NpcUserOptions();
			CurrentNpc = GenerateNpc();
			StateHasChanged();
		}

		public async Task OnNpcList_ViewClicked(int npcId)
		{
			CurrentNpc = await LoadNpc(npcId);
			PageState = CreateNpcPageStates.Details;
			StateHasChanged();
		}

		public void OnShowList_Clicked()
		{
			PageState = CreateNpcPageStates.List;

			PageStateChangedEvent evt = new PageStateChangedEvent
				(
					name: "PageState",
					value: PageState
				);
			NpcEventNotifier.NotifyPropertyChangedAsync(evt)
							.ConfigureAwait(false);
			StateHasChanged();
		}

        public string ShowForPageState(CreateNpcPageStates requiredState)
		{
			return (requiredState == PageState).AsVisibility();
		}

		[Obsolete("This method is marked for deletion.")]
		public string HideWhenPageState(CreateNpcPageStates requiredState)
		{
			return (requiredState != PageState).AsVisibility();
		}

		private void RegisterNpcEventHandlers()
		{
			// Nothing to do here.  Yet.
		}

		private Dictionary<string, string[]> LoadSelectableNpcOptions()
		{
			GuardNpcServicesExists();

			Dictionary<string, string[]>? options = NpcServices!.GetNpcOptions();
			
			return options;
		}

		private NpcClientModel GenerateNpc()
		{
			GuardNpcServicesExists();
			NpcClientModel model;

			model = NpcServices!.GenerateRandomNPC(UserOptions);
			return model;
		}

		private async Task<NpcClientModel> GetAiDescription(NpcClientModel npcModel)
		{
			//TODO:  This Guard => Null Forgiving on dependencies feels bad, man.
			// I won't even remember to do this stuff later until the compiler barks at me.
			// Think of a more elegant  way for components to hold and resolve their own dependencies.
			GuardNpcServicesExists();
			GuardUserExists();

			var user = AppContext!.GetCurrentUser(true);

			npcModel.SetOwner(user!.UserId);

			NpcClientModel npcAi = await NpcServices!.GetAiDescription(npcModel, AppContext);
			return npcAi;
		}

		private async Task<NpcClientModel> SaveNpc(NpcClientModel npcModel)
		{
			GuardNpcServicesExists();
			GuardUserExists();

			// Get the currentUserId.
			var currentUser = AppContext!.GetCurrentUser(true);
			var userLimits = await AppContext.GetUserLimits();


			npcModel.SetOwner(currentUser!.UserId);
			OpResult<NpcClientModel> proxyResult = await NpcServices!.SaveNpc(npcModel, AppContext);

			if(proxyResult.WasSuccessful && proxyResult.Payload !=null)
			{
				return proxyResult.Payload;
			}
			else
			{
				//TODO:  FIgure out how Blazor handles errors once they reach the UI.
				//Also TODO:  Figure out how to get the appsettings.Developer.json pulled in.
				string error = proxyResult.Errors.FirstOrDefault().Value;
				throw new Exception(error);
			}
		}

		private async Task<NpcClientModel> LoadNpc(int npcId)
		{
			GuardNpcServicesExists();
			var proxyResult = await NpcServices!.LoadNpc(npcId);
			if(proxyResult == null)
			{
				throw new Exception("The attempt to load the NPC failed.");
			}
			else
			{
				if (proxyResult.WasSuccessful)
				{
					var newModel = proxyResult.Payload;

					if (newModel != null)
					{
						return newModel;
					}
					else
					{
						throw new Exception($"Could not load an NPC with id {npcId}");
					}
				}
				else
				{
					var sb = new StringBuilder();
					foreach(var item in proxyResult.Errors)
					{
						sb.AppendLine(item.Value);
					}
					throw new Exception(sb.ToString());
				}
				
			}
		}

		private void GuardNpcServicesExists(
			[CallerMemberName] string callerMethod = "",
			[CallerFilePath] string callerFile = "",
			[CallerLineNumber] int callerLineNum = 0)
		{
			if(NpcServices == null)
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
			if(AppContext == null)
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
			if(user == null)
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
