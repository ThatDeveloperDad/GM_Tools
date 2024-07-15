using System;
using System.Text.Json;
using GameTools.TownsfolkManager.Contracts;
using ThatDeveloperDad.Framework.Serialization;

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
        // Convert the Townsperson to JSON
        // use the GetCleanJson method in JsonUtilities
        // to include only those properties that have
        // been populated.
        string npcJson = JsonUtilities.GetCleanJson(npc);

        Console.WriteLine(npcJson);
    }
}
