namespace GameTools.Ruleset.Definitions;

public class BackgroundTemplate
{

    public BackgroundTemplate()
    {
        Name = string.Empty;
    }

    public BackgroundTemplate(string name)
    {
        Name = name;
    }

    public string Name { get; init; }
}
