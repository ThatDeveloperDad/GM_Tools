
using GameTools.Ruleset.Definitions;
using GameTools.Ruleset.Definitions.Characters;

namespace GameTools.Ruleset.DnD5eSRD
{
    public sealed class Species5eProvider : SpeciesData
    {
        public Species5eProvider():base()
        {
            InitializeTemplates();
        }

        protected override void InitializeTemplates()
        {
            SpeciesTemplate elfTemplate = new SpeciesTemplate("Elf");
            RegisterSpecies(elfTemplate);

            SpeciesTemplate dwarfTemplate = new SpeciesTemplate("Dwarf");
            RegisterSpecies(dwarfTemplate);

            
            // [Halfling]  
            SpeciesTemplate halflingTemplate = new SpeciesTemplate("Halfling");
            RegisterSpecies(halflingTemplate);

            // [Human]  

            // [Dragonborn]  

            // [Gnome]  

            // [Half-Elf]  

            // [Half-Orc]

        }
    }
}

