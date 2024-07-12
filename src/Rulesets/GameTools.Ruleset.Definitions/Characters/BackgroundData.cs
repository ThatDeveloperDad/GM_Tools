namespace GameTools.Ruleset.Definitions;

public abstract class BackgroundData : IGameOptionSet<BackgroundTemplate>
{

    protected readonly List<BackgroundTemplate> _backgroundTemplates;

    public BackgroundData()
    {
        _backgroundTemplates = new List<BackgroundTemplate>();
        InitializeTemplates();
    }

    protected void RegisterBackground(BackgroundTemplate background)
    {
        _backgroundTemplates.Add(background);
    }

    protected abstract void InitializeTemplates();

    public string[] List()
    {
        var backgroundList = _backgroundTemplates
            .Select(b=> b.Name)
            .ToArray();

        return backgroundList;
    }

    public BackgroundTemplate? Load(string optionName)
    {
        var ret = _backgroundTemplates
            .FirstOrDefault(x => x.Name == optionName);
        return ret;
    }
}
