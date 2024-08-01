using GameTools.UI.Components.Wrapper;
using GameTools.BlazorClient.Services;
using Microsoft.AspNetCore.Components;

namespace GameTools.BlazorClient.Components.Pages
{
    public class NpcGeneratorComponent:ComponentBase
    {
        [CascadingParameter] 
        protected WaitWrapper? WaitWrapper { get; set; }

        protected DisplayNPCViewModel NPC = new DisplayNPCViewModel();

        [Inject]
        protected NpcViewModelManager npcVmMgr { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            GenerateNpc();
        }

        protected void GenerateNpc()
        {
            DisplayNPCViewModel npcViewModel = npcVmMgr.GenerateRandomNPC();
            NPC = npcViewModel;
        }

        protected async Task OnAiButtonClick()
        {
            NPC = await WaitWrapper.AwaitTask(GetAiDescription());   
        }

        private async Task<DisplayNPCViewModel> GetAiDescription()
        {

            var newModel = await npcVmMgr.GetAiDescription(NPC);
            return newModel??NPC;

        }
    }
}
