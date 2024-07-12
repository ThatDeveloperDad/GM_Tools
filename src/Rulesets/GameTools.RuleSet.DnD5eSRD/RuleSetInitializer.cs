using System.Reflection;
using GameTools.RulesetAccess.Contracts;

namespace GameTools.Ruleset.DnD5eSRD;

public static class RuleSetInitializer
{
    public static IRulesetAccess Use5eSRD(this IRulesetAccess rulesetAccess)
    {
        Type type = typeof(Ruleset5eSRD);

        Assembly rules5eSRD = type.Assembly;
        rulesetAccess.InitializeRuleset(rules5eSRD);
        return rulesetAccess;
    }
}
