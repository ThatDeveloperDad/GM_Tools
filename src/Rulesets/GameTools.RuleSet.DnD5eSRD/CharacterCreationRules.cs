using GameTools.Ruleset.Definitions;
using GameTools.Ruleset.Definitions.Characters;
using GameTools.RuleSet.DnD5eSRD;

namespace GameTools.Ruleset.DnD5eSRD;

public class CharacterCreationRules : ICharacterCreationRules
{
    private readonly Species5eProvider _speciesData;
    private readonly BackgroundData _backgroundData;
    private readonly VocationData _vocationData;

    public CharacterCreationRules()
    {
        _speciesData = new Species5eProvider();
        _backgroundData = new Backgrounds5eData();
        _vocationData = new Vocations5eData();
    }

    public SpeciesData SpeciesRules 
    { 
        get
        {
            return _speciesData;
        } 
    }

    public BackgroundData BackgroundRules
    {
        get
        {
            return _backgroundData;
        }
    }

    public VocationData VocationRules 
    {
        get
        {
            return _vocationData;
        }
    }
}
