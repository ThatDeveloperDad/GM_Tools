namespace GameTools.Ruleset.Definitions;

public class VocationTemplate
{

    public VocationTemplate()
    {
        Name = string.Empty;
    }

    public VocationTemplate(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
