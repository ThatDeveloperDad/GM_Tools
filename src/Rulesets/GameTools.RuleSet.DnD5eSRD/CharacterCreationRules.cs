using GameTools.Ruleset.Definitions;
using GameTools.Ruleset.Definitions.Characters;

namespace GameTools.Ruleset.DnD5eSRD;

public class CharacterCreationRules : ICharacterCreationRules
{
    private readonly Species5eProvider _speciesData;

    public CharacterCreationRules()
    {
        _speciesData = new Species5eProvider();
    }

    public IGameOptionSet<SpeciesTemplate> SpeciesRules 
    { 
        get
        {
            return _speciesData;
        } 
    }
}
