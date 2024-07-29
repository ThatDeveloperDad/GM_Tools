using GameTools.API.WorkloadProvider;

namespace GameTools.BlazorClient.Services
{
    public class NpcViewModelManager
    {
        private readonly ICharacterWorkloads _characterWorker;

        public NpcViewModelManager(ICharacterWorkloads characterWorker)
        {
            _characterWorker = characterWorker;
        }

        public DisplayNPCViewModel GenerateRandomNPC()
        {
            var npc = _characterWorker.GenerateNPC();

            
            DisplayNPCViewModel npcVm = new DisplayNPCViewModel(npc);

            return npcVm;
        }
    }
}
