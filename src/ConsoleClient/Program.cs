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
            string ruleSetAssemblyName = "GameTools.Ruleset.DnD5eSRD";
            Assembly rules = GetAssembly(ruleSetAssemblyName);
            IRulesetAccess ruleSet = new RulesetAccess(rules);

            ITownsfolkManager generator = new TownsfolkManager(ruleSet);
            SpeciesData speciesData = new Species5eProvider();

            var app = new GeneratorConsole(generator, speciesData);
            
            app.TestNPCGen();

            Console.WriteLine("==========");

            app.TestSpeciesList();

        }

        static Assembly GetAssembly(string name)
        {
            // Scan the Current AppDomain for an assembly with the given name.
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly? requested = assemblies.FirstOrDefault(a=> a.GetName().Name == name);
            
            if(requested != null)
            {
                return requested;
            }

            // If the assembly isn't in memory, try to load it from disk.
            var execPath = Assembly.GetExecutingAssembly().Location;
            var dir = System.IO.Path.GetDirectoryName(execPath);
            var path = System.IO.Path.Combine(dir, name + ".dll");
            if (System.IO.File.Exists(path))
            {
                requested = Assembly.LoadFile(path);
            }
            
            if(requested != null)
            {
                return requested;
            }

            throw new InvalidOperationException($"Could not find assembly {name}");
        }
    }
}
