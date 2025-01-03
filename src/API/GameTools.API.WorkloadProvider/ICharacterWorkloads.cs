﻿using GameTools.API.WorkloadProvider.AiWorkloads;
using GameTools.API.WorkloadProvider.Models;
using GameTools.TownsfolkManager.Contracts;
using GameTools.UserManager.Contracts;
using ThatDeveloperDad.Framework.Wrappers;

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
        Townsperson GenerateNPC();

        Townsperson GenerateNPC(TownsfolkUserOptions userOptions);

        /// <summary>
        /// This exists only for the Console application.
        /// Remove it as we get closer to done.
        /// </summary>
        /// <param name="selectedAttributes"></param>
        /// <returns></returns>
        //[Obsolete("This will be going away.  Change over to use TownspersonUserOptions instead.")]
        //Townsperson GenerateNPC(Dictionary<string, string?> selectedAttributes);

        string GetNpcJson(Townsperson npc);

        /// <summary>
        /// Accepts a JsonString with NPC Attributes and passes that to the LLM Service
        /// to generate a description.
        /// </summary>
        /// <param name="npcJson">A JSON string containing the known NPC attributes</param>
        /// <returns>A string with a description of that NPC.</returns>
        [Obsolete("This method is being deprecated.")]
        Task<string> DescribeNPC(Townsperson npc);

        /// <summary>
        /// Calls the LLM, providing the NPC attributes that have already been selected
        /// either randomly or by the user, and generates the values for the properties identified
        /// in the GeneratedCharacterProperties class.
        /// </summary>
        /// <param name="npcJson"></param>
        /// <returns></returns>
        Task<OpResult<ResourceResult<GeneratedCharacterProperties>>> GenerateAttributes(string npcJson, string userId, int userAiQuotaId);

        Task<OpResult<ResourceResult<Townsperson>>> SaveNpc(Townsperson npc, string userId, int userStorageQuotaId); 

        Task<OpResult<IEnumerable<FilteredTownsperson>>> FilterTownsfolk(TownspersonFilter filter);

        Task<OpResult<Townsperson?>> LoadTownsperson(int townspersonId, string userId);

        Task<OpResult<QuotaContainer>> LoadUserQuotas(string userId);
    }
}
