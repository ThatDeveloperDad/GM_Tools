using GameTools.Ruleset.Definitions.Characters;

namespace GameTools.Ruleset.Definitions
{
    public abstract class SpeciesData : IGameOptionSet<SpeciesTemplate>
    {
        protected readonly List<SpeciesTemplate> _speciesTemplates;

        public SpeciesData()
        {
            _speciesTemplates = new List<SpeciesTemplate>();
            InitializeTemplates();
        }


        protected void RegisterSpecies(SpeciesTemplate speciesTemplate)
        {
            _speciesTemplates.Add(speciesTemplate);
        }

        protected abstract void InitializeTemplates();

        public string[] List()
        {
            var speciesList = _speciesTemplates
                    .Select(x => x.Name) 
                    .ToArray();

                return speciesList;
        }

        public SpeciesTemplate? Load(string optionName)
        {
            var ret = _speciesTemplates
                .FirstOrDefault(x => x.Name == optionName);
            return ret;
        }
    }
}