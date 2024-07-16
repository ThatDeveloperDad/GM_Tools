using GameTools.TownsfolkManager.Contracts;
using ThatDeveloperDad.Framework.Serialization;

namespace GameTools.API.WorkloadProvider
{
    public class CharacterWorkloads : ICharacterWorkloads
    {
        private readonly ITownsfolkManager _npcManager;

        public CharacterWorkloads(ITownsfolkManager npcManager)
        {
            _npcManager = npcManager;
        }

        public string GenerateNPC(bool includeAI = false)
        {
            string npcJson = string.Empty;
            var npc = _npcManager.GenerateTownsperson();

            npcJson = Serialize(npc);

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
