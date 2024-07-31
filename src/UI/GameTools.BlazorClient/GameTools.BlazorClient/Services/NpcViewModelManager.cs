using GameTools.API.WorkloadProvider;
using GameTools.API.WorkloadProvider.AiWorkloads;
using GameTools.TownsfolkManager.Contracts;
using ThatDeveloperDad.Framework.Serialization;

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

        public async Task<DisplayNPCViewModel> GetAiDescription(DisplayNPCViewModel npc)
        {
            // First thing we need is a serialized view of the NPC ViewModel.
            string vmJson = npc.SerializeForOutput();

            //string aiDescription = await _characterWorker.DescribeNPC(npc.NpcModel);
            //npc.AddAiDescription(aiDescription);

            GeneratedCharacterProperties genProps = await _characterWorker.GenerateAttributes(vmJson);
            // Need to apply the properties to the inner Townsfolk Model.  lol, oops!
            
            //TODO:  This code is CRAP.  Refactor it for maintainability once it works.
            Townsperson updated = npc.NpcModel;
            updated.FullName.SetAiValue(genProps.Name.ToString());
            updated.Appearance.Description.SetAiValue(genProps.Appearance.ToString());
            updated.PersonalityDescription.SetAiValue(genProps.Personality.ToString());
            updated.Background.Description.SetAiValue(genProps.Background.ToString());
            updated.Vocation.Description.SetAiValue(genProps.CurrentCircumstances.ToString());

            DisplayNPCViewModel updatedVm = new DisplayNPCViewModel(updated);
            return updatedVm;
        }
    }
}
