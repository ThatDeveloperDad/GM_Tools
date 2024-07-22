using GameTools.Framework;
using GameTools.Framework.Concepts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.Ruleset.Definitions.Characters
{
    public static class TemplateHelpers
    {
        public static SpeciesTemplate SetAgeCategories(this SpeciesTemplate tplt, int adult, int retire, int maxAge)
        {
            tplt.SetAgeMilestone(AgeCategoryKind.Adulthood, adult);
            tplt.SetAgeMilestone(AgeCategoryKind.Retirement, retire);
            tplt.SetAgeMilestone(AgeCategoryKind.Lifespan, maxAge);

            return tplt;
        }

        public static SpeciesTemplate SetSpeciesAppearanceOptions(
                        this SpeciesTemplate tplt,
                        IntegumentKind integumentKind,
                        string[] integumentColors,
                        string[] integumentStyles,
                        string[] complexionColors)
        {
            tplt.IntegumentKind = integumentKind;
            tplt.AddIntegumentColors(integumentColors);
            tplt.AddIntegumentStyles(integumentStyles);
            tplt.AddComplexionColors(complexionColors);

            return tplt;
        }
    }
}
