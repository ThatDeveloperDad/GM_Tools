using GameTools.API.WorkloadProvider;
using GameTools.API.WorkloadProvider.AiWorkloads;
using GameTools.TownsfolkManager.Contracts;
using ThatDeveloperDad.Framework.Serialization;

namespace GameTools.BlazorClient.Services
{
    public class NpcServiceProvider
    {
        private readonly ICharacterWorkloads _characterWorker;

        public NpcServiceProvider(ICharacterWorkloads characterWorker)
        {
            _characterWorker = characterWorker;
        }

        public NpcClientModel GenerateRandomNPC()
        {
            var npc = _characterWorker.GenerateNPC();

            
            NpcClientModel npcVm = new NpcClientModel(npc);

            return npcVm;
        }

        public async Task<NpcClientModel> GetAiDescription(NpcClientModel npc)
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

            NpcClientModel updatedVm = new NpcClientModel(updated);
            return updatedVm;
        }
    }
}
