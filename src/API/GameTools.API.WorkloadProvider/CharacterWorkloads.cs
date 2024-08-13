using GameTools.API.WorkloadProvider.AiWorkloads;
using GameTools.TownsfolkManager.Contracts;
using System.Text;
using System.Text.Json;
using ThatDeveloperDad.AIWorkloadManager.Contracts;
using ThatDeveloperDad.Framework.Serialization;
using ThatDeveloperDad.Framework.Wrappers;

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
        public async Task<string> DescribeNPC(Townsperson npc)
        {
            string npcJson = SerializeClean(npc);

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

        public async Task<GeneratedCharacterProperties> GenerateAttributes(string npcJson)
        {
            GeneratedCharacterProperties characterAttributes = new GeneratedCharacterProperties();

            // We need to build the Request Object expected by the AI Workload Manager.
            // We invoke the functions by the names they were registered with.
            
            string functionName = CharGenFunctions.AiFunction_GenerateNPCAttributes;

            // We need to provide a dictionary whose KEYS correspond to the "replacement token" strings
            // in the prompt template.  The values are what we want injected into the prompt template
            // in place of those Replacement Tokens.
            Dictionary<string, object?> functionArgs = new Dictionary<string, object?>();
            functionArgs.Add("npcJson", npcJson);

            // Finally, we invoke that function on the AI Workload provider and await the result.
            var functionResult = await _aiWorker.ExecuteFunctionAsync(functionName, functionArgs);
            string aiJson = string.Empty;

            if (functionResult.IsSuccessful)
            {
                aiJson = functionResult.AiResponse ?? string.Empty;
                aiJson = aiJson.StripMarkdown();
                characterAttributes = ParseFromJson(aiJson);
            }
            else
            {
                //TODO:  Add some logging here.
                aiJson = string.Empty;
            }

            return characterAttributes;
        }

        private GeneratedCharacterProperties ParseFromJson(string json)
        {
            GeneratedCharacterProperties genProps = new GeneratedCharacterProperties();
            // Read the Json into a new Json Document, then interrogate its parts for the 
            // values to assign to the GenProps.
            
            using (var doc = JsonDocument.Parse(json))
            {
                if(doc == null)
                {
                    return genProps;
                }

                //TODO:  This is unmaintainable CRAP, and needs to be made unCRAPpy.
                foreach (var element in doc.RootElement.EnumerateObject())
                {
                    switch(element.Name.Trim().ToUpper())
                    {
                        case "NAME":
                            string nameVal = element.Value.GetString() ?? string.Empty;
                            genProps.Name.SetAiValue(nameVal);
                            break;

                        case "PERSONALITY":
                            string personalityVal = element.Value.GetString() ?? string.Empty;
                            genProps.Personality.SetAiValue(personalityVal);
                            break;

                        case "APPEARANCE":
                            string looksVal = element.Value.GetString() ?? string.Empty;
                            genProps.Appearance.SetAiValue(looksVal);
                            break;

                        case "CURRENTCIRCUMSTANCES":
                            string jobVal = element.Value.GetString() ?? string.Empty;
                            genProps.CurrentCircumstances.SetAiValue(jobVal);
                            break;

                        case "BACKGROUND":
                            string histVal = element.Value.GetString() ?? string.Empty;
                            genProps.Background.SetAiValue(histVal);
                            break;

                        default:
                            //Do Nothing.
                            break;
                    }
                }
            }

            return genProps;
        }

        /// <summary>
        /// Randomly generates an NPC from the available options provided by
        /// the configured RuleSet.
        /// </summary>
        /// <param name="includeAI"></param>
        /// <returns></returns>
        public Townsperson GenerateNPC()
        {
            string npcJson = string.Empty;
            var npc = _npcManager.GenerateTownsperson();

            

            return npc;
        }

        /// <summary>
        /// This overload exists only for the console application.
        /// </summary>
        /// <param name="selectedAttributes"></param>
        /// <returns></returns>
        public Townsperson GenerateNPC(Dictionary<string, string?> selectedAttributes)
        {
            var npc = _npcManager.GenerateTownspersonFromOptions(selectedAttributes);

            return npc;
        }

        public Townsperson GenerateNPC(TownsfolkUserOptions options)
        {
            Townsperson npc = _npcManager.GenerateTownspersonFromOptions(options);
            return npc;
        }

        public string GetNpcJson(Townsperson npc)
        {
            string npcJson = SerializeClean(npc);
            return npcJson;
        }

        public async Task<OpResult<Townsperson>> SaveNpc(Townsperson npc)
        {
            var managerResult = await _npcManager.SaveTownsperson(npc);
            return managerResult;
        }

        public async Task<OpResult<IEnumerable<FilteredTownsperson>>> FilterTownsfolk(TownspersonFilter filter)
        {
            var apiResult = await _npcManager.FilterTownspeople(filter);
            return apiResult;
        }

        /// <summary>
        /// Serializes an object to JSON
        /// Uses the JsonFunctions to strip all properties
        /// from a complex object that have not been populated.
        /// </summary>
        /// <typeparam name="T">The Type of object</typeparam>
        /// <param name="instance">An Instance of T</param>
        /// <returns></returns>
        private string SerializeClean<T>(T instance) where T : class
        {
            string json = JsonUtilities.GetCleanJson(instance);

            return json;
        }

        
    }
}
