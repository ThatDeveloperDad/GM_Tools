using GameTools.API.WorkloadProvider;
using GameTools.API.WorkloadProvider.AiWorkloads;
using GameTools.BlazorClient.Components;
using GameTools.BlazorClient.Middleware;
using GameTools.BlazorClient.Services.MappingExtensions;
using GameTools.Framework.Contexts;
using GameTools.TownsfolkManager.Contracts;
using Microsoft.Graph.Models;
using ThatDeveloperDad.Framework.Serialization;
using ThatDeveloperDad.Framework.Wrappers;

namespace GameTools.BlazorClient.Services
{
    public class NpcServiceProxy
    {
        private readonly ICharacterWorkloads _npcApi;
        private readonly ILogger<NpcServiceProxy>? _logger;

        public NpcServiceProxy(ICharacterWorkloads characterProxy,
            ILogger<NpcServiceProxy>? logger)
        {
            _npcApi = characterProxy;
            _logger = logger;
        }

        public Dictionary<string, string[]> GetNpcOptions()
        {
            Dictionary<string, string[]> options = _npcApi.GetNpcOptions();
            return options;
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

        public async Task<NpcClientModel> GetAiDescription(NpcClientModel npc, AppStateProvider appCtx)
        {
			var userLimits = await appCtx.GetUserLimits();
			int userAiQuotaId = userLimits.NpcAiDescriptions.QuotaId;

			// First thing we need is a serialized view of the NPC ViewModel.
			string vmJson = npc.SerializeForOutput();
            var apiResult = await _npcApi.GenerateAttributes(vmJson, npc.OwnerId, userAiQuotaId);

			Townsperson updated = npc.NpcModel;
			GeneratedCharacterProperties? genAiAttributes = apiResult?.Payload?.Result;
			if (genAiAttributes != null)
            {
				updated.FullName.SetAiValue(genAiAttributes.Name.ToString());
				updated.Appearance.Description.SetAiValue(genAiAttributes.Appearance.ToString());
				updated.PersonalityDescription.SetAiValue(genAiAttributes.Personality.ToString());
				updated.Background.Description.SetAiValue(genAiAttributes.Background.ToString());
				updated.Vocation.Description.SetAiValue(genAiAttributes.CurrentCircumstances.ToString());
			}

            var updatedQuotas = apiResult?.Payload?.UpdatedQuotas;
            if (updatedQuotas != null)
            {
                var uiQuotas = updatedQuotas.ToUiModel();
                appCtx?.SetUserLimits(uiQuotas);
            }

			NpcClientModel updatedVm = new NpcClientModel(updated);
            return updatedVm;
        }

        public async Task<OpResult<NpcClientModel>> SaveNpc(NpcClientModel npc, AppStateProvider appCtx )
        {
            var userQuotas = await appCtx.GetUserLimits();
            int userStorageQuotaId = userQuotas.NpcsInStorage.QuotaId;

			OpResult<NpcClientModel> proxyResult = new OpResult<NpcClientModel>();
            
            Townsperson npcData = npc.NpcModel;
            
            var apiResult = await _npcApi.SaveNpc(npcData, npc.OwnerId, userStorageQuotaId);

            if(apiResult.WasSuccessful)
            {
				if (apiResult?.Payload?.Result == null)
				{
                    Guid errorId = Guid.NewGuid();
                    string nullNpcMsg = "The Save NPC operation did not return the latest version of this NPC.";
                    proxyResult.AddError(errorId, nullNpcMsg);

                    return proxyResult;
				}
				
                Townsperson apiPayload = apiResult.Payload.Result;
                
                NpcClientModel proxyPayload = new NpcClientModel(apiPayload);
                proxyResult.Payload = proxyPayload;

                var resultQuotas = apiResult?.Payload?.UpdatedQuotas;

				if (resultQuotas != null)
                {
                    var uiLimits = resultQuotas!.ToUiModel();
                    appCtx?.SetUserLimits(uiLimits);
                }
            }
            else
            {
                apiResult.CopyErrorsTo(ref proxyResult);
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
                UserId = filter.UserId,
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

        internal async Task<OpResult<UserLimitsModel>> LoadUserLimits(string userId)
        {
            OpResult<UserLimitsModel> proxyResult = new OpResult<UserLimitsModel>();
            UserLimitsModel? proxyPayload = null;

            try
            {
                var apiResult = await _npcApi.LoadUserQuotas(userId);
                if(apiResult.WasSuccessful && apiResult.Payload != null)
                {
                    var apiPayload = apiResult.Payload;
                    proxyPayload = apiPayload.ToUiModel();
                }
                else
                {
                    apiResult.CopyErrorsTo(ref proxyResult);
                }
            }
            catch(Exception ex)
            {
                Guid exId = Guid.NewGuid();
                string message = $"Could not load the Quotas for user {userId}";

                _logger?.LogError(ex, exId.ToString());
                proxyResult.AddError(exId, message);
            }

            proxyResult.Payload = proxyPayload;
            return proxyResult;
        }
    }
}
