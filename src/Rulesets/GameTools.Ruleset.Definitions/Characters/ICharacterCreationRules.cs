
namespace GameTools.Ruleset.Definitions.Characters;

public interface ICharacterCreationRules
    {
        // Choose Species Template
        /// <summary>
        /// Retrieves a list of species we can use when building a character.
        /// </summary>
        /// <returns>An array of species names.</returns>
        
        /// <summary>
        /// Provides access to the available species and the rules that govern 
        /// using those species when building a character.
        /// </summary>
        IGameOptionSet<SpeciesTemplate> SpeciesRules {get;}
        
#region ToBeImplemented
        // Choose Background Template
        //string[] ListBackgrounds();
        // BackgroundTemplate LoadBackgroundTemplate(string backgroundName);


        // Choose Vocation Template
        //string[] ListVocations();
        // VocationTemplate LoadVocationTemplate(string vocationName);
#endregion
    }
