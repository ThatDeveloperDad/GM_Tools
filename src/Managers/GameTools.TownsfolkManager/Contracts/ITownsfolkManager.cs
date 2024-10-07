using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThatDeveloperDad.Framework.Wrappers;

namespace GameTools.TownsfolkManager.Contracts
{
    /// <summary>
    /// Describes the behavior of a component that allows us to generate, store, and retrieve Townsfolk for our
    /// game worlds.
    /// </summary>
    public interface ITownsfolkManager
    {
        /// <summary>
        /// Creates a new Townsperson and populates its properties with randomly generated or chosen values.
        /// </summary>
        /// <returns></returns>
        Townsperson GenerateTownsperson();
        
        /// <summary>
        /// Creates a new Townsperson, using any options supplied from the end user,
        /// and choosing randomly for those options that are not supplied.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Townsperson GenerateTownspersonFromOptions(TownsfolkUserOptions options);

        /// <summary>
        /// Retrieves the dictionary of options that are used when creating an
        /// NPC.
        /// </summary>
        /// <returns>A keyed dictionary of character generation options where the key contains the Category of the Templates, and the value is a list of the template Names.</returns>
        Dictionary<string, string[]> GetNpcOptions();

        /// <summary>
        /// Stores the provided Townsperson instance in a data storage location.
        /// </summary>
        /// <param name="townsperson">The NPC instance to save.</param>
        /// <returns>Returns a Task with an OpResult carrying the ID of the townsperson that was persisted.</returns>
        Task<OpResult<Townsperson>> SaveTownsperson(Townsperson townsperson);

        /// <summary>
        /// Retrieves the identified Townsperson instance from the configured storage location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        Task<OpResult<Townsperson?>> LoadTownsperson(int npcId);

        /// <summary>
        /// Returns a list of all Townsperson instances currently stored in the configured storage location.
        /// </summary>
        /// <returns></returns>
        Task<OpResult<IEnumerable<FilteredTownsperson>>> FilterTownspeople(TownspersonFilter filter);
    }
}