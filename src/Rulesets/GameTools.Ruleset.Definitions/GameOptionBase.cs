namespace GameTools.Ruleset.Definitions;

/// <summary>
/// Defines the absolute minimum shared features of any option for any rule in a TTRPG system's RuleSet.
/// </summary>
public abstract class GameOptionBase
{
    protected GameOptionBase(string optionName)
    {
        Name = optionName;
    }

    public string Name { get; private set; }
}
