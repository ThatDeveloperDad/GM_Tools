using GameTools.API.WorkloadProvider;
using GameTools.API.WorkloadProvider.AiWorkloads;
using GameTools.TownsfolkManager.Contracts;
using ThatDeveloperDad.Framework.Serialization;
using ThatDeveloperDad.Framework.Wrappers;

namespace GameTools.BlazorClient.Services
{
    public class NpcServiceProvider
    {
        private readonly ICharacterWorkloads _characterWorker;

        public NpcServiceProvider(ICharacterWorkloads characterWorker)
        {
            _characterWorker = characterWorker;
        }

        public Dictionary<string, string[]> GetNpcOptions()
        {
            Dictionary<string, string[]> options = _characterWorker.GetNpcOptions();
            return options;
        }

        public NpcClientModel GenerateRandomNPC(Dictionary<string, string?>? selectedOptions = null)
        {
            Townsperson npc;

            if(selectedOptions == null)
            {
                npc = _characterWorker.GenerateNPC();
			}
            else
            {
                npc = _characterWorker.GenerateNPC(selectedOptions);
            }
            
            NpcClientModel npcVm = new NpcClientModel(npc);

            return npcVm;
        }

        public NpcClientModel GenerateRandomNPC(NpcUserOptions userOptions)
        {
            TownsfolkUserOptions options = new TownsfolkUserOptions();
            
            options.Species = userOptions.Species;
            options.Gender = userOptions.Gender;
            options.Background = userOptions.Background;
            options.Vocation = userOptions.Vocation;
            options.IsRetired = userOptions.IsRetired;

            Townsperson npc = _characterWorker.GenerateNPC(options);


            NpcClientModel npcVm = new NpcClientModel(npc);
            return npcVm;
        }

        public async Task<NpcClientModel> GetAiDescription(NpcClientModel npc)
        {
            // First thing we need is a serialized view of the NPC ViewModel.
            string vmJson = npc.SerializeForOutput();

            //string aiDescription = await _characterWorker.DescribeNPC(npc.NpcModel);
            //npc.AddAiDescription(aiDescription);

            GeneratedCharacterProperties genAiAttributes = await _characterWorker.GenerateAttributes(vmJson);
            // Need to apply the properties to the inner Townsfolk Model.  lol, oops!
            
            //TODO:  This code is CRAP.  Refactor it for maintainability once it works.
            Townsperson updated = npc.NpcModel;
            updated.FullName.SetAiValue(genAiAttributes.Name.ToString());
            updated.Appearance.Description.SetAiValue(genAiAttributes.Appearance.ToString());
            updated.PersonalityDescription.SetAiValue(genAiAttributes.Personality.ToString());
            updated.Background.Description.SetAiValue(genAiAttributes.Background.ToString());
            updated.Vocation.Description.SetAiValue(genAiAttributes.CurrentCircumstances.ToString());

            NpcClientModel updatedVm = new NpcClientModel(updated);
            return updatedVm;
        }

        public async Task<OpResult<NpcClientModel>> SaveNpc(NpcClientModel npc)
        {
            OpResult<NpcClientModel> proxyResult = new OpResult<NpcClientModel>();
            
            Townsperson npcData = npc.NpcModel;
            var apiResult = await _characterWorker.SaveNpc(npcData);

            if(apiResult.WasSuccessful)
            {
                Townsperson? apiPayload = apiResult.Result;
                NpcClientModel proxyPayload = new NpcClientModel(apiPayload);
                proxyResult.Result = proxyPayload;
            }
            else
            {
                foreach(var kvp in apiResult.Errors)
                {
                    proxyResult.AddError(kvp.Key, kvp.Value);
                }
            }

            return proxyResult;
        }
    }
}
