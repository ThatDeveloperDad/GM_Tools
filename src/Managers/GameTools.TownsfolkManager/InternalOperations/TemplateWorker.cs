using GameTools.DiceEngine;
using GameTools.Ruleset.Definitions;
using GameTools.Ruleset.Definitions.Characters;
using GameTools.TownsfolkManager.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.TownsfolkManager.InternalOperations
{
    internal static class TemplateWorker
    {
        public static Townsperson ApplySpecies(this Townsperson npc, 
                                                    SpeciesTemplate template,
                                                    ICardDeck shuffler,
                                                    IDiceBag dice)
        {
            npc.Species = template.Name;

            // Choose Gender from the options on the Template.
            string genderOption = shuffler.PickOne(template.GenderOptions);
            npc.Appearance.Gender = genderOption;
            npc.Pronouns = template.GetPronouns(genderOption);

            return npc;
        }

        public static Townsperson ApplyBackground(this Townsperson npc, 
                                                       BackgroundTemplate template,
                                                       ICardDeck shuffler,
                                                       IDiceBag dice)
        {
            npc.Background.Name = template.Name;
            return npc; 
        }

        public static Townsperson ApplyVocation(this Townsperson npc, 
                                                     VocationTemplate template,
                                                     ICardDeck shuffler,
                                                     IDiceBag dice)
        {
            npc.Vocation.Name = template.Name;
            return npc; 
        }
    }
}
