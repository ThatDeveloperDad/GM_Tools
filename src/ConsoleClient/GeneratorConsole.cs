using System;
using System.Text.Json;
using GameTools.TownsfolkManager.Contracts;

namespace ConsoleClient;

public class GeneratorConsole
{
    private readonly ITownsfolkManager _generator;

    public GeneratorConsole(ITownsfolkManager townsfolkManager)
    {
        _generator = townsfolkManager;
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
