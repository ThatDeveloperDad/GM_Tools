namespace GameTools.Ruleset.Definitions;

public abstract class VocationData : IGameOptionSet<VocationTemplate>
{

    protected readonly List<VocationTemplate> _vocationTemplates;

    public VocationData()
    {
        _vocationTemplates = new List<VocationTemplate>();
        InitializeTemplates();
    }

    protected abstract void InitializeTemplates();

    protected void RegisterVocation(VocationTemplate vocation)
    {
        _vocationTemplates.Add(vocation);
    }

    public string[] List()
    {
        var vocations = _vocationTemplates
            .Select(x => x.Name)
            .ToArray();

        return vocations;
    }

    public VocationTemplate? Load(string optionName)
    {
        var ret = _vocationTemplates
            .FirstOrDefault(x => x.Name == optionName);
        return ret;
    }
}
