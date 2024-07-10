using GameTools.Ruleset.Definitions.Characters;

namespace GameTools.Ruleset.Definitions
{
    public abstract class SpeciesData
    {
        protected readonly List<SpeciesTemplate> _speciesTemplates;

        public SpeciesData()
        {
            _speciesTemplates = new List<SpeciesTemplate>();
        }

        public string[] ListSpecies
        {
            get
            {
                var speciesList = _speciesTemplates
                    .Select(x => x.Name) 
                    .ToArray();

                return speciesList;
            }
        }

        protected void RegisterSpecies(SpeciesTemplate speciesTemplate)
        {
            _speciesTemplates.Add(speciesTemplate);
        }

        protected abstract void InitializeTemplates();
    }
}