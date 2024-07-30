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
            try
            {
                NPC = await WaitWrapper.AwaitTask(GetAiDescription());
            }
            catch (Exception e)
            {
                // Don't ever do this.  I'm only try catching to see what's actually happening.
                throw;
            }
        }
        private async Task<string> FakeTask()
        {
            await Task.Delay(3000);
            return string.Empty;
        }

        private async Task<DisplayNPCViewModel> GetAiDescription()
        {

            var newModel = await npcVmMgr.GetAiDescription(NPC);
            return newModel??NPC;

        }
    }
}
