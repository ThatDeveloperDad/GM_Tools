using Microsoft.Extensions.DependencyInjection;
using GameTools.DiceEngine;
using GameTools.RulesetAccess.Contracts;
using GameTools.RulesetAccess;
using GameTools.Ruleset.DnD5eSRD;
using GameTools.TownsfolkManager;
using GameTools.TownsfolkManager.Contracts;
using GameTools.API.WorkloadProvider;
using System;
using ThatDeveloperDad.AIWorkloadManager.Contracts;
using ThatDeveloperDad.AIWorkloadManager;
using ThatDeveloperDad.LlmAccess.Contracts;
using ThatDeveloperDad.LlmAccess;

// See https://aka.ms/new-console-template for more information
namespace ConsoleClient
{
    class Program
    {
        static void Main()
        {

            var services = CreateServices();

            ICharacterWorkloads app = BuildApp(services);

            string npcJson = app.GenerateNPC();
            Console.WriteLine(npcJson);

        }

        static ICharacterWorkloads BuildApp(ServiceProvider services)
        {
            ITownsfolkManager tfMgr = services.GetRequiredService<ITownsfolkManager>();
            IAiWorkloadManager aiMgr = services.GetRequiredService<IAiWorkloadManager>();

            ICharacterWorkloads app = new CharacterWorkloads(tfMgr, aiMgr);

            return app;
        }

        static ServiceProvider CreateServices()
        {
            var services = new ServiceCollection();

            #region TownsFolk Subsystem
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
            #endregion

            #region AI Subsystem

            services.AddScoped<IPromptExecution, PromptExecution>();
            services.AddScoped<IAiWorkloadManager, AiWorkloadManager>();


            #endregion

            return services.BuildServiceProvider();
        }

    }
}
