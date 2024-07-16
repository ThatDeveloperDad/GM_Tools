using GameTools.TownsfolkManager.Contracts;
using ThatDeveloperDad.AIWorkloadManager.Contracts;
using ThatDeveloperDad.Framework.Serialization;

namespace GameTools.API.WorkloadProvider
{
    public class CharacterWorkloads : ICharacterWorkloads
    {
        private readonly ITownsfolkManager _npcManager;
        private readonly IAiWorkloadManager _llmProvider;

        public CharacterWorkloads(ITownsfolkManager npcManager,
                                  IAiWorkloadManager llmProvider)
        {
            _npcManager = npcManager;
            _llmProvider = llmProvider;
        }

        public string GenerateNPC(bool includeAI = false)
        {
            string npcJson = string.Empty;
            var npc = _npcManager.GenerateTownsperson();

            npcJson = Serialize(npc);

            if(includeAI)
            {
                string description = _llmProvider.DescribeNPC(npcJson);
            }

            return npcJson;
        }

        /// <summary>
        /// Serializes an object to JSON
        /// Uses the JsonFunctions to strip all properties
        /// from a complex object that have not been populated.
        /// </summary>
        /// <typeparam name="T">The Type of object</typeparam>
        /// <param name="instance">An Instance of T</param>
        /// <returns></returns>
        private string Serialize<T>(T instance) where T : class
        {
            string json = JsonUtilities.GetCleanJson(instance);

            return json;
        }
    }
}
