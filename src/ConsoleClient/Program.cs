using Microsoft.Extensions.DependencyInjection;
using GameTools.DiceEngine;
using GameTools.RulesetAccess.Contracts;
using GameTools.RulesetAccess;
using GameTools.Ruleset.DnD5eSRD;
using GameTools.TownsfolkManager;
using GameTools.TownsfolkManager.Contracts;
using GameTools.API.WorkloadProvider;
using System;

// See https://aka.ms/new-console-template for more information
namespace ConsoleClient
{
    class Program
    {
        static void Main()
        {

            var services = CreateServices();

            //var app = new GeneratorConsole(services.GetRequiredService<ITownsfolkManager>());

            ICharacterWorkloads app = new CharacterWorkloads(services.GetRequiredService<ITownsfolkManager>()); 

            string npcJson =  app.GenerateNPC();
            Console.WriteLine(npcJson);
        }

        static ServiceProvider CreateServices()
        {
            var services = new ServiceCollection();

            services.AddScoped<ITownsfolkManager, TownsfolkManager>();

            services.AddScoped<IRulesetAccess>((sp) => 
                {
                    IRulesetAccess service = 
                        new RulesetAccess()
                            .Use5eSRD();

                    return service;
                });

            services.AddScoped<ICardDeck, DeckSimulator>();

            services.AddScoped<IDiceBag, DiceBag>();


            return services.BuildServiceProvider();
        }

    }
}
