using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ThatDeveloperDad.Framework.Wrappers;
using GameTools.NPCAccess.SqlServer.Context;
using GameTools.NPCAccess.SqlServer.Context.SqlModels;
using GameTools.NPCAccess.SqlServer.Transformers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GameTools.NPCAccess.SqlServer
{
    public class NpcAccessSqlProvider : INpcAccess, IDisposable
    {
        private ILogger<NpcAccessSqlProvider> _logger;
        private readonly string _userDataCn;
        private NpcAccessDbContext? _ctx;
        
        public NpcAccessSqlProvider(ILogger<NpcAccessSqlProvider> logger,
                                    string userDataCn)
        {
            _logger = logger;
            _userDataCn = userDataCn;
        }

        private NpcAccessDbContext Ctx
        {
            get
            {
                _ctx = _ctx ?? new NpcAccessDbContext(_userDataCn);
                return _ctx;
            }
        }

        /// <summary>
        /// Queries the NPC Storage for all NPCs that match the filter properties.
        /// </summary>
        /// <param name="filter">A collection of nullable properties that will be used to build the where clause for the NPC Query.</param>
        /// <returns></returns>
        public async Task<OpResult<IEnumerable<NpcAccessFilterResult>>> FilterNpcs(NpcAccessFilter filter)
        {
            List<NpcAccessFilterResult> filteredNpcs = new List<NpcAccessFilterResult>();
            OpResult<IEnumerable<NpcAccessFilterResult>> accessResult = new OpResult<IEnumerable<NpcAccessFilterResult>>();
            accessResult.Payload = filteredNpcs;

            try
            {
                IQueryable<NpcRowModel> query 
                    = Ctx.Npcs
                         .Where(n=> n.DeletedDate.HasValue == false);
                
                string? userIdFilter = filter.UserId;
				string? speciesFilter = filter.Species;
                string? vocationFilter = filter.Vocation;
                
                if(string.IsNullOrWhiteSpace(userIdFilter) == false)
                {
                    query = query.Where(n => n.UserId.ToUpper() == userIdFilter.ToUpper());
                }

                if(string.IsNullOrWhiteSpace(speciesFilter) == false)
                {
                    query = query.Where(n=> n.SpeciesName.ToUpper() == speciesFilter.ToUpper());
                }

                if (string.IsNullOrWhiteSpace(vocationFilter) == false)
                {
                    query = query.Where(n=> n.VocationName.ToUpper() == vocationFilter.ToUpper());
                }

                // apply a default sort to bring the most recently created ones to the top.
                query = query.OrderByDescending(c => c.CreatedDate);


                //var rowModels = query.ToList();

                var rowModels = await query.ToListAsync()
                                           .ConfigureAwait(false);

                foreach(var row in rowModels)
                {
                    NpcAccessFilterResult filteredResult = new NpcAccessFilterResult
                        (id: row.NpcId,
                         species: row.SpeciesName,
                         vocation: row.VocationName,
                         name: row.CharacterName??string.Empty);
                    filteredNpcs.Add(filteredResult);
                }

            }
            catch(Exception ex)
            {
                Guid errorId = Guid.NewGuid();
                string errorMessage = $"There was a problem accessing the NPCs.";
                _logger.LogError(ex.Message
                                , new
                                {
                                    ErrorId = errorId,
                                    ExceptionType = ex.GetType().Name,
                                    TimeStampUTC = DateTime.UtcNow,
                                    Site = $"{nameof(NpcAccessSqlProvider)}.{FilterNpcs}"
                                });
                accessResult.AddError(errorId, errorMessage);
            }

            return accessResult;
        }

        public async Task<OpResult<int>> SaveNpc(NpcAccessModel npc)
        {
            OpResult<int> result = new OpResult<int>();

            try
            {
                NpcRowModel? model = await Ctx.Npcs.FindAsync(npc.NpcId);

                if (model == null)
                {
                    // Set the metadata for a new Npc and Add it to the context.
                    model = npc.ToRowModel();
                    model.CreatedDate = DateTime.Now;
                    Ctx.Add(model);
                }
                else
                {
                    // Apply the incoming changes and Update the context.
                    model = model.ApplyInboundChanges(npc);
                    model.UpdatedDate = DateTime.Now;
                    Ctx.Update(model);
                }

                await Ctx.SaveChangesAsync();
                result.Payload = model.NpcId;
            }
            catch (Exception e)
            {
                Guid errorId = Guid.NewGuid();
                string errorMessage = $"Could not save the NPC.";
                _logger.LogError(e.Message
                                , new
                                {
                                    ErrorId = errorId,
                                    ExceptionType = e.GetType().Name,
                                    TimeStampUTC = DateTime.UtcNow,
                                    Site= $"{nameof(NpcAccessSqlProvider)}.{SaveNpc}"
                                });
                result.AddError(errorId, errorMessage);
            }
            
            return result;
        }

        public async Task<OpResult<NpcAccessModel?>> LoadNpc(int npcId)
        {
            NpcAccessModel? accessPayload = null;
            OpResult<NpcAccessModel?> accessResult = new OpResult<NpcAccessModel?>(accessPayload);

            try
            {
                var sqlModel = await Ctx.Npcs.FindAsync(npcId);

                if(sqlModel == null)
                {
                    accessResult.AddError(Guid.NewGuid(), $"The requested Npc with id {npcId} was not found.");
                }
                else
                {
                    accessPayload = sqlModel.ToDto();
                    accessResult.Payload = accessPayload;
                }
            }
            catch(Exception ex)
            {
				Guid errorId = Guid.NewGuid();
				string errorMessage = $"Could not save the NPC.";
				_logger.LogError(ex.Message
								, new
								{
									ErrorId = errorId,
									ExceptionType = ex.GetType().Name,
									TimeStampUTC = DateTime.UtcNow,
									Site = $"{nameof(NpcAccessSqlProvider)}.{LoadNpc}"
								});
				accessResult.AddError(errorId, errorMessage);
			}

            return accessResult;

        }

        #region IDisposable Implementation

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _ctx?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        

        #endregion // IDisposable
    }
}
