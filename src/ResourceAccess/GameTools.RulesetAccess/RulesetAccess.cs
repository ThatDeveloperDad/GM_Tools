using System;
using System.Linq;
using System.Reflection;
using GameTools.Ruleset.Definitions.Characters;
using GameTools.RulesetAccess.Contracts;

namespace GameTools.RulesetAccess;

public class RulesetAccess : IRulesetAccess
{
    private readonly Assembly _ruleDefinitions;



    public RulesetAccess(Assembly ruleDefinitions)
    {
        _ruleDefinitions = ruleDefinitions;

    }

    public ICharacterCreationRules LoadCharacterCreationRules()
    {
        ICharacterCreationRules rules = LoadImplementation<ICharacterCreationRules>();
        return rules;
    }

    private T LoadImplementation<T>()
    {
        Type abstractType = typeof(T);
        Type? concreteType = _ruleDefinitions.GetTypes()
                                            .FirstOrDefault(ct => ct.GetInterfaces()
                                                                    .Contains(abstractType));

        if (concreteType == null)
        {
            throw new ArgumentException($"The requested service {abstractType.Name} could not be found in {_ruleDefinitions.GetName().Name}.");
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
