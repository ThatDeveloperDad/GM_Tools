
using GameTools.Framework.Concepts;
using GameTools.Ruleset.Definitions;
using GameTools.Ruleset.Definitions.Characters;
using System.Runtime.CompilerServices;

namespace GameTools.Ruleset.DnD5eSRD
{
    public sealed class Species5eProvider : SpeciesData
    {
        public Species5eProvider():base()
        {
            
        }

        private SpeciesTemplate CreateTemplate(string speciesName,
            int avgHeight, int heightVariance, int avgWeight, int weightVariance)
        {
            SpeciesTemplate template = new SpeciesTemplate(speciesName);
            template.AverageHeightCm = avgHeight;
            template.HeightVarianceCm = heightVariance;
            template.AverageWeightKg = avgWeight;
            template.WeightVarianceKg = weightVariance;

            return template;
        }

        

        protected override void InitializeTemplates()
        {
            SpeciesTemplate dwarfTemplate =
                CreateTemplate("Dwarf",
                    avgHeight: 137, avgWeight: 68,
                    heightVariance: 15, weightVariance: 20)
                .SetAgeCategories(50, 250, 350);
            RegisterSpecies(dwarfTemplate);

            SpeciesTemplate elfTemplate =
                CreateTemplate("Elf",
                    avgHeight: 167, avgWeight: 59,
                    heightVariance: 15, weightVariance: 10)
                .SetAgeCategories(100, 600, 750);
            RegisterSpecies(elfTemplate);

            SpeciesTemplate halflingTemplate =
                CreateTemplate("Halfling",
                    avgHeight: 91, avgWeight: 18,
                    heightVariance: 30, weightVariance: 5)
                .SetAgeCategories(20, 90, 150);
            RegisterSpecies(halflingTemplate);

            SpeciesTemplate humanTemplate =
                CreateTemplate("Human",
                    avgHeight: 167, avgWeight: 64,
                    heightVariance: 15, weightVariance: 25)
                .SetAgeCategories(18, 70, 100);
            RegisterSpecies(humanTemplate);

            SpeciesTemplate dragonbornTemplate =
                CreateTemplate("Dragonborn",
                    avgHeight: 198, avgWeight: 113,
                    heightVariance: 15, weightVariance: 20)
                .SetAgeCategories(15, 60, 80);
            RegisterSpecies(dragonbornTemplate);

            SpeciesTemplate gnomeTemplate =
                CreateTemplate("Gnome",
                    avgHeight: 107, avgWeight: 18,
                    heightVariance: 15, weightVariance: 5)
                .SetAgeCategories(40, 300, 500);
            RegisterSpecies(gnomeTemplate);

            SpeciesTemplate halfElfTemplate =
                CreateTemplate("Half-Elf",
                    avgHeight: 167, avgWeight: 64,
                    heightVariance: 15, weightVariance: 25)
                .SetAgeCategories(20, 150, 180);
            RegisterSpecies(halfElfTemplate);

            SpeciesTemplate halfOrcTemplate =
                CreateTemplate("Half-Orc",
                    avgHeight: 183, avgWeight: 82,
                    heightVariance: 15, weightVariance: 25)
                .SetAgeCategories(14, 60, 75);
            RegisterSpecies(halfOrcTemplate);

        }
    }
}

