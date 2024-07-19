using System.Collections.Generic;

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

        Townsperson GenerateTownspersonFromOptions(Dictionary<string, string?> selectedAttributes);

        /// <summary>
        /// Retrieves the dictionary of options that are used when creating an
        /// NPC.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string[]> GetNpcOptions();

        /// <summary>
        /// Stores the provided Townsperson instance in a data storage location.
        /// </summary>
        /// <param name="townsperson">The NPC instance to save.</param>
        /// <returns>The location of the saved data.  Could be a URI or a File Path, depending on the implementation.</returns>
        string SaveTownsperson(Townsperson townsperson);

        /// <summary>
        /// Retrieves the identified Townsperson instance from the configured storage location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        Townsperson LoadTownsperson(string location);

        /// <summary>
        /// Returns a list of all Townsperson instances currently stored in the configured storage location.
        /// </summary>
        /// <returns></returns>
        Townsperson[] ListTownspersons();
    }
}