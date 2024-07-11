using System;
using System.Text.Json;
using GameTools.Ruleset.Definitions;
using GameTools.Ruleset.DnD5eSRD;
using GameTools.TownsfolkManager;
using GameTools.TownsfolkManager.Contracts;

namespace ConsoleClient;

public class GeneratorConsole
{
    private readonly ITownsfolkManager _generator;
    private readonly SpeciesData _speciesData;

    public GeneratorConsole(ITownsfolkManager townsfolkManager,
                            SpeciesData speciesData)
    {
        _generator = townsfolkManager;
        _speciesData = speciesData;
    }

    public void TestSpeciesList()
    {
        var speciesList = _speciesData.List();
        foreach (var species in speciesList)
        {
            Console.WriteLine(species);
        }
    }

    public void TestNPCGen()
    {
        // Generate a single townsperson
        Townsperson npc = _generator.GenerateTownsperson();
        PrintNPC(npc);
    }

    void PrintNPC(Townsperson npc)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        string npcJson = JsonSerializer.Serialize(npc, options);
        Console.WriteLine(npcJson);
    }
}
