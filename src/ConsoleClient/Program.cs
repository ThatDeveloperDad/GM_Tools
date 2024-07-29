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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata;

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
            string yesTrigger = "y";
            string[] yesNoOptions ={ yesTrigger, "n" };
            

            string continuePrompt = $"Type {stopPhrase} to exit, or anything else to make another.";
            string[] continuePromptOptions = Array.Empty<string>();

            var npcOptions = app.GetNpcOptions();

            bool promptToSelectNPCOptions = npcOptions.Count > 0;
            string selectNpcOptionsPrompt = "Do you want to pick any of the NPC Attributes?";

            while (userInput.ToUpper() != stopPhrase.ToUpper() )
            {
                string npcJson = string.Empty;
                Townsperson? npc = null;

                if(promptToSelectNPCOptions == true)
                {
                    string userSelects = PromptUser(selectNpcOptionsPrompt, yesNoOptions);
                    if(userSelects.Trim().ToUpper() == yesTrigger.ToUpper())
                    {
                        var selectedOptions = new Dictionary<string, string?>();
                        foreach(var entry in npcOptions)
                        {
                            selectedOptions.Add(entry.Key, null);
                        }

                        // For each attribute that we have a list of options for, do this thing.
                        foreach(var entry in npcOptions)
                        {
                            string attributeName = entry.Key;
                            string[] attributeOptions = entry.Value;

                            Console.WriteLine("==============");
                            Console.WriteLine($"Select {attributeName}");
                            string selectOptionPrompt = $"Choose the {attributeName} from the following list.";
                            Console.WriteLine(selectOptionPrompt);
                            
                            // Generates a numbered list that shows each AttributeOption
                            for (int itemIndex = 0; itemIndex < attributeOptions.Length; itemIndex++)
                            {
                                string optionText = $"{itemIndex + 1}:  {attributeOptions[itemIndex]} | ";
                                Console.Write(optionText);
                            }
                            Console.WriteLine(" or press [ENTER] to leave it random.");
                            string? choice = Console.ReadLine();
                            int? choiceIndex = null;
                            if(int.TryParse(choice ?? "", out int parsedIndex) && parsedIndex>0)
                            {
                                // Remember, when we display the choices, we're adding 1 to the array index.
                                choiceIndex = parsedIndex - 1;
                                selectedOptions[attributeName] = entry.Value[choiceIndex ?? 0];
                            }

                        }
                        npc = app.GenerateNPC();
                        npcJson = app.GetNpcJson(npc);
                    }
                }
               
                if(npc == null)
                {
                    npc = app.GenerateNPC();
                    npcJson = app.GetNpcJson(npc);
                }

                Console.WriteLine("Here'a preview of the NPC.");
                Console.WriteLine(npcJson);

                userInput = PromptUser(generateDescriptionPrompt, yesNoOptions);
                if(userInput.ToUpper() == yesTrigger.ToUpper())
                {
                    Console.WriteLine("This may take a few seconds.  Please be patient.");
                    Console.WriteLine();
                    
                    string description = app.DescribeNPC(npc).Result;
                    Console.WriteLine("Attributes:");
                    Console.WriteLine(npcJson);
                    Console.WriteLine();
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
            services.AddScoped<ITownsfolkManager, TownsfolkMgr>();

            services.AddScoped<IRulesetAccess>((sp) => 
                {
                    IRulesetAccess service = 
                        new RulesetProvider()
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
