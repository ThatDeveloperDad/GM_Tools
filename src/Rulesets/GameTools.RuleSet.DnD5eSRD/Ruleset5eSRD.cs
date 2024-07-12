using GameTools.Ruleset.Definitions;

namespace GameTools.Ruleset.DnD5eSRD;

public class Ruleset5eSRD : RulesetBase
{
    protected override void RegisterSpeciesData()
    {
        this.Species = new Species5eProvider();
    }
}
