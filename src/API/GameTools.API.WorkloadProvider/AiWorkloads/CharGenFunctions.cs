using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatDeveloperDad.AIWorkloadManager.Contracts;

namespace GameTools.API.WorkloadProvider.AiWorkloads
{
    /// <summary>
    /// Added this to keep the Function Definitions and Registration out of the main
    /// CharacterWorkload classes.  Just a tidy code thing.
    /// </summary>
    internal static class CharGenFunctions
    {

        public static void RegisterCharacterGenerationFunctions(IAiWorkloadManager workloadMgr)
        {
            var describeNPC = BuildFunction_DescribeNPC();
            workloadMgr.RegisterAiFunction(describeNPC);
        }

        public const string AiFunction_DescribeNPC = "DescribeNPC";
        private static AiFunctionDefinition BuildFunction_DescribeNPC()
        {
            string name = AiFunction_DescribeNPC;
            string description = "Given a Json string containing seed data, generates a text description of the character.";
            string promptTemplate = @"
You are a helpful assistant to a busy Game Master.
For this request, you will use the Seed Attributes that are provided as
a JSON string to create a new Character for a Fantasy setting.

Please create a suitable name for this character, and BRIEF descriptions
of thier appearance, personality, and current circumstances.  Keep your 
descriptions short and to the point, maybe one or two sentences for each
category.  Finally, please add a 3 or 4 sentence paragraph describing how
their background contributed to their current personality and circumstance.

Seed Attributes
============
{{$npcJson}}
============
"; 

            AiFunctionDefinition functionDef = new AiFunctionDefinition(
                name: name, 
                description: description, 
                template: promptTemplate);

            return functionDef;
        }

        
    }
}
