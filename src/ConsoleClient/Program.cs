using GameTools.TownsfolkManager;
using GameTools.TownsfolkManager.Contracts;
using GameTools.Ruleset.DnD5eSRD;
using System;
using GameTools.Ruleset.Definitions;
using GameTools.RulesetAccess.Contracts;
using GameTools.RulesetAccess;
using System.Reflection;
using System.Linq;

// See https://aka.ms/new-console-template for more information
namespace ConsoleClient
{
    class Program
    {
        static void Main()
        {
            IRulesetAccess ruleSet = 
                new RulesetAccess()
                    .Use5eSRD();
           
            ITownsfolkManager generator = new TownsfolkManager(ruleSet);
            SpeciesData speciesData = ruleSet
                                        .LoadCharacterCreationRules()
                                        .SpeciesRules;

            var app = new GeneratorConsole(generator, speciesData);
            
            app.TestNPCGen();

            Console.WriteLine("==========");

            app.TestSpeciesList();

        }

    }
}
