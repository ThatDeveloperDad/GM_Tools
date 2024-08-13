﻿using ThatDeveloperDad.Framework.Wrappers;

namespace GameTools.NPCAccess
{
    public interface INpcAccess
    {
        /// <summary>
        /// Receives an instance of the NPC model, saves it to storage,
        /// and returns its identifier if successful.
        /// </summary>
        /// <param name="npc"></param>
        /// <returns>Task with an int result.</returns>
        Task<OpResult<int>> SaveNpc(NpcAccessModel npc);

        /// <summary>
        /// Returns an Enumerable collection of NPCFilterResult objects
        /// that match the provided filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<OpResult<IEnumerable<NpcAccessFilterResult>>> FilterNpcs(NpcAccessFilter filter);
    }
}
