namespace GameTools.Ruleset.Definitions.Characters
{
    // Defines the properties that are common to the concept of a TTRPG Species.
    public class SpeciesTemplate
    {
        public SpeciesTemplate()
        {
            Name = string.Empty;
        }

        public SpeciesTemplate(string name)
        {
            Name = name;
        }
        
        public string Name { get; init; }
    }
}