using System;

namespace GameTools.RulesetAccess.Contracts
{
    public interface IRulesetAccess
    {
        // Q:  What does a RulesetAccess component do?
        // A:  It provides access to specific implementations of 
        //     operations within a given set of TTRPG Game Rules.

        // Q: What kind of rules exist within a TTRPG Ruleset?
        // A: Character Generation Rules <== We only care about those.
        // A: Combat Rules
        // A: Action Rules
        // A: Resource Management Rules

        object LoadCharacterGeneratorRules();
    }
}

