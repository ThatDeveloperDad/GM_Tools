
using GameTools.Framework;
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
                .SetAgeCategories(50, 250, 350)
                .SetSpeciesAppearanceOptions(IntegumentKind.Hair,
                    integumentColors: ["Brown", "Black", "Red", "Grey", "Auburn"],
                    integumentStyles: ["Thick", "Braided"],
                    complexionColors: ["Light", "Tan", "Deep Tan", "Pale"]);
            RegisterSpecies(dwarfTemplate);

            SpeciesTemplate elfTemplate =
                CreateTemplate("Elf",
                    avgHeight: 167, avgWeight: 59,
                    heightVariance: 15, weightVariance: 10)
                .SetAgeCategories(100, 600, 750)
                .SetSpeciesAppearanceOptions(IntegumentKind.Hair,
                    integumentColors: [ "Blonde", "Brown", "Black", "Silver", "Green", "Blue" ],
                    integumentStyles: [ "Long", "Silky" ],
                    complexionColors: [ "Pale", "Light Brown", "Bronze" ])
;
            RegisterSpecies(elfTemplate);

            SpeciesTemplate halflingTemplate =
                CreateTemplate("Halfling",
                    avgHeight: 91, avgWeight: 18,
                    heightVariance: 30, weightVariance: 5)
                .SetAgeCategories(20, 90, 150)
                .SetSpeciesAppearanceOptions(IntegumentKind.Hair,
                    integumentColors: new string[] { "Brown", "Blonde", "Black", "Auburn" },
                    integumentStyles: new string[] { "Curly", "Neat" },
                    complexionColors: new string[] { "Light", "Olive", "Tan" })
;
            RegisterSpecies(halflingTemplate);

            SpeciesTemplate humanTemplate =
                CreateTemplate("Human",
                    avgHeight: 167, avgWeight: 64,
                    heightVariance: 15, weightVariance: 25)
                .SetAgeCategories(18, 70, 100)
                .SetSpeciesAppearanceOptions(IntegumentKind.Hair,
                    integumentColors: new string[] { "Brown", "Blonde", "Black", "Red", "Grey" },
                    integumentStyles: new string[] { "Long", "Balding", "Close-Cropped", "Straight", "Wavy" },
                    complexionColors: new string[] { "Fair", "Light", "Tan", "Brown", "Dark Brown" })
;
            RegisterSpecies(humanTemplate);

            SpeciesTemplate dragonbornTemplate =
                CreateTemplate("Dragonborn",
                    avgHeight: 198, avgWeight: 113,
                    heightVariance: 15, weightVariance: 20)
                .SetAgeCategories(15, 60, 80)
                .SetSpeciesAppearanceOptions(IntegumentKind.Scales,
                    integumentColors: new string[] { "Gold", "Red", "Blue", "Green", "Black", "Silver", "Bronze" },
                    integumentStyles: new string[] { "Shiny", "Thick" },
                    complexionColors: new string[] {});
            RegisterSpecies(dragonbornTemplate);

            SpeciesTemplate gnomeTemplate =
                CreateTemplate("Gnome",
                    avgHeight: 107, avgWeight: 18,
                    heightVariance: 15, weightVariance: 5)
                .SetAgeCategories(40, 300, 500)
                .SetSpeciesAppearanceOptions(IntegumentKind.Hair,
                    integumentColors: new string[] { "White", "Brown", "Blonde", "Black", "Red", "Pink", "Green" },
                    integumentStyles: new string[] { "Wispy", "Curly" },
                    complexionColors: new string[] { "Light", "Tan", "Deep Tan" })
;
            RegisterSpecies(gnomeTemplate);

            SpeciesTemplate halfElfTemplate =
                CreateTemplate("Half-Elf",
                    avgHeight: 167, avgWeight: 64,
                    heightVariance: 15, weightVariance: 25)
                .SetAgeCategories(20, 150, 180)
                .SetSpeciesAppearanceOptions(IntegumentKind.Hair,
                    integumentColors: new string[] { "Blonde", "Brown", "Black", "Silver", "Blue", "Purple" },
                    integumentStyles: new string[] { "Long", "Balding", "Close-Cropped", "Straight", "Wavy" },
                    complexionColors: new string[] { "Fair", "Light Brown", "Bronze", "Pale" })
;
            RegisterSpecies(halfElfTemplate);

            SpeciesTemplate halfOrcTemplate =
                CreateTemplate("Half-Orc",
                    avgHeight: 183, avgWeight: 82,
                    heightVariance: 15, weightVariance: 25)
                .SetAgeCategories(14, 60, 75)
                .SetSpeciesAppearanceOptions(IntegumentKind.Hair,
                    integumentColors: new string[] { "Black", "Brown", "Grey" },
                    integumentStyles: new string[] { "Short", "Coarse" },
                    complexionColors: new string[] { "Gray-Green", "Dark Green", "Black" })
;
            RegisterSpecies(halfOrcTemplate);

        }
    }
}

