using GameTools.DiceEngine;
using GameTools.Framework.Concepts;
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
            npc = npc.SetBodyAttributes(template, shuffler, dice)
                     .SetAgeAttributes(template, dice)
                     .SetAppearanceAttributes(template, shuffler);

            return npc;
        }

        private static Townsperson SetAgeAttributes(this Townsperson npc, SpeciesTemplate template, IDiceBag dice)
        {
            // For NPC Age, let's see if they're retired or not.
            bool isRetired = dice.CoinToss();
            int minAge;
            int maxAge;
            if (isRetired)
            {
                npc.Vocation.IsRetired = true;
                minAge = template.AgeMilestones[AgeCategoryKind.Retirement].GetValueOrDefault();
                maxAge = template.AgeMilestones[AgeCategoryKind.Lifespan].GetValueOrDefault();
            }
            else
            {
                minAge = template.AgeMilestones[AgeCategoryKind.Adulthood].GetValueOrDefault();
                maxAge = template.AgeMilestones[AgeCategoryKind.Retirement].GetValueOrDefault();
            }

            minAge = dice.ApplyFuzzFactor(minAge, 10);
            maxAge = dice.ApplyFuzzFactor(maxAge, 10);

            npc.AgeYears = dice.GetRandomBetween(minAge, maxAge);

            return npc;
        }

        private static Townsperson SetBodyAttributes(this Townsperson npc, SpeciesTemplate template, ICardDeck shuffler, IDiceBag dice)
        {
            string genderOption = shuffler.PickOne(template.GenderOptions);
            npc.Appearance.Gender = genderOption;
            npc.Pronouns = template.GetPronouns(genderOption);

            npc.Appearance.HeightCm = dice.ApplyVarianceRange(template.AverageHeightCm, template.HeightVarianceCm);
            npc.Appearance.WeightKg = dice.ApplyVarianceRange(template.AverageWeightKg, template.WeightVarianceKg);

            return npc;
        }

        private static Townsperson SetAppearanceAttributes(this Townsperson npc, SpeciesTemplate tplt, ICardDeck shuffler)
        {
            npc.Appearance.IntegumentKind = tplt.IntegumentKind;
            npc.Appearance.IntegumentColor = shuffler.PickOne(tplt.IntegumentColorOptions);
            npc.Appearance.IntegumentStyle = shuffler.PickOne(tplt.IntegumentStyleOptions);
            npc.Appearance.Complexion = shuffler.PickOne(tplt.ComplexionColors);

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
