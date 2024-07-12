using GameTools.Ruleset.Definitions.Characters;

namespace GameTools.Ruleset.Definitions;

public abstract class RulesetBase
{
    public RulesetBase(){}

    public bool IsReady{
        get
        {
            bool isReady = 
                (
                    Species != null
                );
            return isReady;
        }
    }

    public IGameOptionSet<SpeciesTemplate>? Species {get; protected set; }

    protected abstract void RegisterSpeciesData();
    
}
