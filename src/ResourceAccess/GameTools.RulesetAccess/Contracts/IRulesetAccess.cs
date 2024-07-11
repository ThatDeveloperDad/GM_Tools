using System;
using GameTools.Ruleset.Definitions.Characters;

namespace GameTools.RulesetAccess.Contracts;

public interface IRulesetAccess
{
    ICharacterCreationRules LoadCharacterCreationRules();
}
