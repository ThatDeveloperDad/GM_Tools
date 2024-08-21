using GameTools.API.WorkloadProvider;
using GameTools.API.WorkloadProvider.AiWorkloads;
using GameTools.TownsfolkManager.Contracts;
using ThatDeveloperDad.Framework.Serialization;
using ThatDeveloperDad.Framework.Wrappers;

namespace GameTools.BlazorClient.Services
{
    public class NpcServiceProxy
    {
        private readonly ICharacterWorkloads _npcApi;

        public NpcServiceProxy(ICharacterWorkloads characterProxy)
        {
            _npcApi = characterProxy;
        }

        public Dictionary<string, string[]> GetNpcOptions()
        {
            Dictionary<string, string[]> options = _npcApi.GetNpcOptions();
            return options;
        }

        //TODO:  Delete this, and the old blazor component that uses it.
        public NpcClientModel GenerateRandomNPC(Dictionary<string, string?>? selectedOptions = null)
        {
            Townsperson npc;

            if(selectedOptions == null)
            {
                npc = _npcApi.GenerateNPC();
			}
            else
            {
                npc = _npcApi.GenerateNPC(selectedOptions);
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

            Townsperson npc = _npcApi.GenerateNPC(options);


            NpcClientModel npcVm = new NpcClientModel(npc);
            return npcVm;
        }

        public async Task<NpcClientModel> GetAiDescription(NpcClientModel npc)
        {
            // First thing we need is a serialized view of the NPC ViewModel.
            string vmJson = npc.SerializeForOutput();

            //string aiDescription = await _characterWorker.DescribeNPC(npc.NpcModel);
            //npc.AddAiDescription(aiDescription);

            GeneratedCharacterProperties genAiAttributes = await _npcApi.GenerateAttributes(vmJson);
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
            var apiResult = await _npcApi.SaveNpc(npcData);

            if(apiResult.WasSuccessful)
            {
                Townsperson? apiPayload = apiResult.Payload;
                NpcClientModel proxyPayload = new NpcClientModel(apiPayload);
                proxyResult.Payload = proxyPayload;
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

        public async Task<OpResult<IEnumerable<NpcFilterRowModel>>> FilterTownsfolk(NpcClientFilter filter)
        {
            List<NpcFilterRowModel> proxyPayload = new List<NpcFilterRowModel>();
            OpResult<IEnumerable<NpcFilterRowModel>> proxyResult = new OpResult<IEnumerable<NpcFilterRowModel>>(proxyPayload);

            // Convert the NpcCLientFIlter into the expected Api Format.
            TownspersonFilter apiFilter = new TownspersonFilter
            {
                Species = filter.Species,
                Vocation = filter.Vocation,
            };

            // Call & Await the API.
            var apiResult = await _npcApi.FilterTownsfolk(apiFilter);

            // Convert the result back to the client model types.
            if(apiResult == null)
            {
                proxyResult.AddError(Guid.NewGuid(), "Something very bad happened.");
            }
            else
            {
                if(apiResult.WasSuccessful)
                {
                    var apiPayload = apiResult.Payload ?? Array.Empty<FilteredTownsperson>();
                    foreach(var listItem in apiPayload)
                    {
                        NpcFilterRowModel row = new NpcFilterRowModel
                            (npcId: listItem.NpcId,
                             name: listItem.Name,
                             species: listItem.Species,
                             vocation: listItem.Vocation);

                        proxyPayload.Add(row);
                    }
                }
                else
                {
                    foreach(var kvp in apiResult.Errors)
                    {
                        proxyResult.AddError(kvp.Key, kvp.Value);
                    }
                }
            }

            // Send it back to the invoking PageModel.
            return proxyResult;
        }

        public async Task<OpResult<NpcClientModel?>> LoadNpc(int npcId)
        {
            NpcClientModel? proxyPayload = null;
            OpResult<NpcClientModel?> proxyResult = new OpResult<NpcClientModel?>(proxyPayload);

            var apiResult = await _npcApi.LoadTownsperson(npcId);

            //TODO:  This is where we left off on Wednesday, 8/14.
            // Pick up troubleshooting here.
            if(apiResult == null)
            {
                proxyResult.AddError(Guid.NewGuid(), "The API did not return any result.");
            }
            else
            {
                if(apiResult.WasSuccessful)
                {
                    var apiPayload = apiResult.Payload;
                    if(apiPayload !=null)
                    {
						proxyPayload = new NpcClientModel(apiPayload);
                        proxyResult.Payload = proxyPayload;
					}
                    else
                    {
                        proxyResult.AddError(Guid.NewGuid(), $"The Api did not return the Npc with Id {npcId}.");
                    }
                }
                else
                {
                    foreach(var kvp in apiResult.Errors)
                    {
                        proxyResult.AddError(kvp.Key, kvp.Value);
                    }
                }
            }

            return proxyResult;
        }
    }
}
