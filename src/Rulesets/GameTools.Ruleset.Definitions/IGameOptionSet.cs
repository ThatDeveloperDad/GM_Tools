namespace GameTools.Ruleset.Definitions;

public interface IGameOptionSet<T>
{

    string[] List();

    T? Load(string optionName);
}
