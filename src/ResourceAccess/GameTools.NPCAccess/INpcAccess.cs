using ThatDeveloperDad.Framework.Wrappers;

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

        /// <summary>
        /// Fetches the identified npc from storage, and returns it to the caller as
        /// an NPCAccess model.  It is up to the caller to translate the json in the 
        /// CharacterDetails property into its local representation of the NPC.
        /// </summary>
        /// <param name="npcId">The ID of the npc to be retrieved.</param>
        /// <returns>An instance of OpResult, which will have any error messages that were encountered during the retrieval operation, or an instance of the NpcAccessModel, if found.</returns>
        Task<OpResult<NpcAccessModel?>> LoadNpc(int npcId);
    }
}
