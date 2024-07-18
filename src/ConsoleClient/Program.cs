﻿using Microsoft.Extensions.DependencyInjection;
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
using System.Linq;
using System.Collections;
using System.Collections.Generic;

// See https://aka.ms/new-console-template for more information
namespace ConsoleClient
{
    class Program
    {
        static void Main()
        {

            var services = CreateServices();

            ICharacterWorkloads app = BuildApp(services);

            // Instead of assuming that we want to include the
            // AI Generated Description, let's show a preview of
            // the NPC Attributes, and ASK the user if they want to
            // get the AI Description.
            // Let's also do this in a loop so that we can just run this
            // application until the user says, "I'm done."

            string stopPhrase = "stop";
            string userInput = string.Empty;

            string generateDescriptionPrompt = "Do you want an AI Description of this character?";
            string generateDescriptionTrigger = "y";
            string[] generateDescriptionOptions ={ generateDescriptionTrigger, "n" };
            

            string continuePrompt = $"Type {stopPhrase} to exit, or anything else to make another.";
            string[] continuePromptOptions = Array.Empty<string>();

            while (userInput.ToUpper() != stopPhrase.ToUpper() )
            {
                string npcJson = app.GenerateNPC();
                Console.WriteLine("Here's your new NPC.");
                Console.WriteLine(npcJson);

                userInput = PromptUser(generateDescriptionPrompt, generateDescriptionOptions);
                if(userInput.ToUpper() == generateDescriptionTrigger.ToUpper())
                {
                    Console.WriteLine("This may take a few seconds.  Please be patient.");
                    Console.WriteLine();
                    string description = app.DescribeNPC(npcJson).Result;
                    Console.WriteLine("NPC Description");
                    Console.WriteLine();
                    Console.WriteLine(description);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("*************************************");
                    Console.WriteLine();
                }

                userInput = PromptUser(continuePrompt, continuePromptOptions);
                Console.WriteLine("*************************************");
                Console.WriteLine();
            }

        }

        static string PromptUser(string promptText, string[] validAnswers)
        {
            List<string> promptOptions = validAnswers
                .Select(s=> s.Trim().ToUpper())
                .ToList();

            string answersText = $"Choose from {string.Join(",", validAnswers)}  :  ";
            

            string answer = string.Empty;

            if(promptOptions.Any() == true)
            {
                Console.WriteLine(promptText);
                while (promptOptions.Contains(answer.Trim().ToUpper()) == false)
                {
                    Console.Write(answersText);
                    answer = Console.ReadLine() ?? string.Empty;
                }
            }
            else
            {
                Console.Write(promptText);
                answer = Console.ReadLine() ?? string.Empty;
            }
            
            return answer;
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

            services.AddScoped<ILlmProvider, LlmProvider>();
            services.AddScoped<IAiWorkloadManager, AiWorkloadManager>();


            #endregion

            return services.BuildServiceProvider();
        }

    }
}
