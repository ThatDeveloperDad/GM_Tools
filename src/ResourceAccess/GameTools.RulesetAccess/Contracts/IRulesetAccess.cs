using System;
using System.Reflection;
using GameTools.Ruleset.Definitions.Characters;

namespace GameTools.RulesetAccess.Contracts;

public interface IRulesetAccess
{
    void InitializeRuleset(Assembly ruleSet);
    
    ICharacterCreationRules LoadCharacterCreationRules();
}
