using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ThatDeveloperDad.Framework.Wrappers;
using GameTools.NPCAccess.SqlServer.Context;
using GameTools.NPCAccess.SqlServer.Context.SqlModels;
using GameTools.NPCAccess.SqlServer.Transformers;

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
                result.Result = model.NpcId;
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
