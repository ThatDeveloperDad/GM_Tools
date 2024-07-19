using GameTools.API.WorkloadProvider.AiWorkloads;
using GameTools.TownsfolkManager.Contracts;
using System.Text;
using ThatDeveloperDad.AIWorkloadManager.Contracts;
using ThatDeveloperDad.Framework.Serialization;

namespace GameTools.API.WorkloadProvider
{
    public class CharacterWorkloads : ICharacterWorkloads
    {
        private readonly ITownsfolkManager _npcManager;
        private readonly IAiWorkloadManager _aiWorker;

        public CharacterWorkloads(ITownsfolkManager npcManager,
                                  IAiWorkloadManager aiWorker)
        {
            _npcManager = npcManager;
            _aiWorker = aiWorker;

            // Register the DescribeNPC function with the LLM Provider.
            CharGenFunctions.RegisterCharacterGenerationFunctions(_aiWorker);
        }

        /// <summary>
        /// Retrieves a Dictionary of the different options that can 
        /// be used when generating an NPC.
        /// The Dictionary Key identifies the NPC Attribute.
        /// The Value of a Dictionary Entry is the List of Choices.
        /// </summary>
        public Dictionary<string, string[]> GetNpcOptions()
        {
            var options = _npcManager.GetNpcOptions();

            return options;
        }

        /// <summary>
        /// Passes the NPC Data to an LLM to generate a detailed description.
        /// </summary>
        /// <param name="npcJson"></param>
        /// <returns></returns>
        public async Task<string> DescribeNPC(string npcJson)
        {
            // We need to build the Request Object expected by the AI Workload Manager.
            // We invoke the functions by the names they were registered with.
            string functionName = CharGenFunctions.AiFunction_DescribeNPC;

            // We need to provide a dictionary whose KEYS correspond to the "replacement token" strings
            // in the prompt template.  The values are what we want injected into the prompt template
            // in place of those Replacement Tokens.
            Dictionary<string, object?> functionArgs = new Dictionary<string, object?>();
            functionArgs.Add("npcJson", npcJson);

            // Finally, we invoke that function on the AI Workload provider and await the result.
            var functionResult = await _aiWorker.ExecuteFunctionAsync(functionName, functionArgs);
            string description = string.Empty;

            // If it's successful, we should have an answer from the AI.
            // If it's not, the output of THIS method is going to the UI.
            // Let's just return an empty string when the AI Workload fails.
            if (functionResult.IsSuccessful)
            {
                description = functionResult.AiResponse??string.Empty;
            }
            else
            {
                //TODO:  Add some logging here.
                description = string.Empty;
            }

            return description;
        }

        /// <summary>
        /// Randomly generates an NPC from the available options provided by
        /// the configured RuleSet.
        /// </summary>
        /// <param name="includeAI"></param>
        /// <returns></returns>
        public string GenerateNPC()
        {
            string npcJson = string.Empty;
            var npc = _npcManager.GenerateTownsperson();

            npcJson = Serialize(npc);

            return npcJson;
        }

        public string GenerateNPC(Dictionary<string, string?> selectedAttributes)
        {
            string npcJson = string.Empty;
            var npc = _npcManager.GenerateTownspersonFromOptions(selectedAttributes);

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
