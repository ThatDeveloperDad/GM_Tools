using GameTools.Ruleset.Definitions.Characters;

namespace GameTools.RulesetAccess
{
    public interface IGeneratorRules
    {
        // Choose Species Template
        /// <summary>
        /// Retrieves a list of species we can use when building a character.
        /// </summary>
        /// <returns>An array of species names.</returns>
        string[] GetSpecies();

        SpeciesTemplate ChooseSpeciesTemplate();
        // How can we choose a Species?
        //   Pass in its name, selected from a list.
        //   Count the available choices, and generate a random number and pick that choice.
        //   Some other way.

        // Choose Background Template

        // Choose Vocation Template
    }
}