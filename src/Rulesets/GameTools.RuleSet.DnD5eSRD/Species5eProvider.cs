
using GameTools.Ruleset.Definitions;
using GameTools.Ruleset.Definitions.Characters;

namespace GameTools.Ruleset.DnD5eSRD
{
    public sealed class Species5eProvider : SpeciesData
    {
        public Species5eProvider():base()
        {
            
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
            SpeciesTemplate humanTemplate = new SpeciesTemplate("Human");
            RegisterSpecies(humanTemplate);

            // [Dragonborn]  
            SpeciesTemplate dragonbornTemplate = new SpeciesTemplate("Dragonborn");
            RegisterSpecies(dragonbornTemplate);

            // [Gnome]  
            SpeciesTemplate gnomeTemplate = new SpeciesTemplate("Gnome");
            RegisterSpecies(gnomeTemplate);

            // [Half-Elf]  
            SpeciesTemplate halfElfTemplate = new SpeciesTemplate("Half-Elf");
            RegisterSpecies(halfElfTemplate);

            // [Half-Orc]
            SpeciesTemplate halfOrcTemplate = new SpeciesTemplate("Half-Orc");
            RegisterSpecies(halfOrcTemplate);

        }
    }
}

