using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.API.WorkloadProvider
{
    /// <summary>
    /// Describes the behaviors provided by a component that supplies Character Generation capabilities.
    /// </summary>
    public interface ICharacterWorkloads
    {
        /// <summary>
        /// Retrieves a Dictionary of the different options that can 
        /// be used when generating an NPC.
        /// The Dictionary Key identifies the NPC Attribute.
        /// The Value of a Dictionary Entry is the List of Choices.
        /// </summary>
        Dictionary<string, string[]> GetNpcOptions();

        /// <summary>
        /// Generates an NPC randomly from the configured Ruleset.
        /// Optionally, passes the NPC Object to an LLM to get a more detailed
        /// description.
        /// </summary>
        /// <param name="includeAI">If true, additionally sends the generated NPC to the LLM</param>
        /// <returns>The Generated NPC with or without the AI description.</returns>
        string GenerateNPC();

        string GenerateNPC(Dictionary<string, string?> selectedAttributes);

        /// <summary>
        /// Accepts a JsonString with NPC Attributes and passes that to the LLM Service
        /// to generate a description.
        /// </summary>
        /// <param name="npcJson">A JSON string containing the known NPC attributes</param>
        /// <returns>A string with a description of that NPC.</returns>
        Task<string> DescribeNPC(string npcJson);
    }
}
