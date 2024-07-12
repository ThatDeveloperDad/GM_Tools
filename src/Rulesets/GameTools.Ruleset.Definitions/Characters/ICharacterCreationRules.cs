
namespace GameTools.Ruleset.Definitions.Characters;

public interface ICharacterCreationRules
    {
        
        
        /// <summary>
        /// Provides access to the available species and the rules that govern 
        /// using those species when building a character.
        /// </summary>
        SpeciesData SpeciesRules {get;}
        
        /// <summary>
        /// Provides access to the backgrounds that are available in the 
        /// implemented ruleSet.
        /// </summary>
        BackgroundData BackgroundRules {get;}

        /// <summary>
        /// Provides access to the vocations that are available in the 
        /// implemented ruleSet.
        /// </summary>
        VocationData VocationRules {get;}
    }
