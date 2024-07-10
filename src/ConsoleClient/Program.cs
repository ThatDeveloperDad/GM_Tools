using GameTools.RuleSet.DnD5eSRD;
using GameTools.TownsfolkManager;
using GameTools.TownsfolkManager.Contracts;
using System;
using System.Text.Json;

// See https://aka.ms/new-console-template for more information
namespace ConsoleClient
{
    class Program
    {
        static void Main()
        {
            ITownsfolkManager generator = new TownsfolkManager();

            var ruleSet5e = new Species5eProvider();
            TestSpeciesList(ruleSet5e);


        }

        static void TestSpeciesList(Species5eProvider ruleSet5e)
        {
            var speciesList = ruleSet5e.ListSpecies;
            foreach (var species in speciesList)
            {
                Console.WriteLine(species);
            }
        }

        static void TestNPCGen(ITownsfolkManager generator)
        {
            // Generate a single townsperson
            Townsperson npc = generator.GenerateTownsperson();
            PrintNPC(npc);
        }

        static void PrintNPC(Townsperson npc)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string npcJson = JsonSerializer.Serialize(npc, options);
            Console.WriteLine(npcJson);
        }
    }
}
