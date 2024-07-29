using System;
using System.Linq;
using System.Reflection;
using GameTools.Ruleset.Definitions.Characters;
using GameTools.RulesetAccess.Contracts;

namespace GameTools.RulesetAccess;

public class RulesetProvider : IRulesetAccess
{
    private Assembly? _ruleDefinitions;

    public RulesetProvider(Assembly? ruleDefinitions = null)
    {
        _ruleDefinitions = ruleDefinitions;
    }

    public void InitializeRuleset(Assembly ruleSet)
    {
        _ruleDefinitions = ruleSet;
    }

    private ICharacterCreationRules? _charGenRules;
    public ICharacterCreationRules LoadCharacterCreationRules()
    {
        if(_charGenRules == null)
        {
            _charGenRules = ResolveContract<ICharacterCreationRules>();    
        }
        
        return _charGenRules;
    }

    private T ResolveContract<T>()
    {
        if(_ruleDefinitions == null)
        {
            throw new InvalidOperationException("The rule set has not been initialized.");
        }
        
        Type contractType = typeof(T);
        Type? concreteType = _ruleDefinitions.GetTypes()
                                            .FirstOrDefault(ct => ct.GetInterfaces()
                                                                    .Contains(contractType));

        if (concreteType == null)
        {
            throw new ArgumentException($"The requested service {contractType.Name} could not be found in {_ruleDefinitions.GetName().Name}.");
        }

        var instance = Activator.CreateInstance(concreteType);
        if(instance == null)
        {
            throw new ArgumentException("The requested service could not be created.");
        }

        T requestedService = (T)instance;

    

        return requestedService;                                                          
    }
}
