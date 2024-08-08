
using Microsoft.Extensions.Logging;

namespace GameTools.NPCAccess.SqlServer
{
    public class NpcAccessSqlProvider : INpcAccess
    {
        private ILogger<NpcAccessSqlProvider> _logger;
        private readonly string _userDataCn;

        public NpcAccessSqlProvider(ILogger<NpcAccessSqlProvider> logger,
                                    string userDataCn)
        {
            _logger = logger;
            _userDataCn = userDataCn;
        }

        public async Task<int> SaveNpc(NpcAccessModel npc)
        {
            throw new NotImplementedException();
        }
    }
}
