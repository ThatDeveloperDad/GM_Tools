using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.MeteredUsageAccess.ResourceModels
{
    /// <summary>
    /// We'll record Quota usage in broad terms.
    /// 
    /// But!  Each GameEntity will consume a different amount of
    /// each resource.  (i.e.:  NPCs consume fewer AI Tokens and
    /// Storage than say, a Town, or Tavern)
    /// </summary>
    public enum MeteredResourceKind
    {
        NpcStorage,
        NpcAiDetail
    }
}
