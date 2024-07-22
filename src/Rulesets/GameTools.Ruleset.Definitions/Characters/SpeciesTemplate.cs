using System.ComponentModel.DataAnnotations;

namespace GameTools.Ruleset.Definitions.Characters
{
    // Defines the properties that are common to the concept of a TTRPG Species.
    public class SpeciesTemplate
    {
        public readonly string[] DefaultGenderChoices = [ "Ambiguous", "Female", "Male" ];
        public readonly Dictionary<string, string> DefaultPronounMapping;
        public readonly List<string> _customGenderOptions;
        public readonly Dictionary<string, string> _customPronounMappings;

        public SpeciesTemplate():this(string.Empty) { }

        public SpeciesTemplate(string name)
        {
            Name = name;
            _customGenderOptions = new List<string>();
            _customPronounMappings = new Dictionary<string, string>();

            DefaultPronounMapping = new Dictionary<string, string>();
            DefaultPronounMapping["Ambiguous"] = "They/Them/Theirs";
            DefaultPronounMapping["Female"] = "She/Her/Hers";
            DefaultPronounMapping["Male"] = "He/Him/His";
        }
        
        public string Name { get; init; }

        public void AddGenderOptions(Tuple<string, string>[] genderOptions)
        {
            var genders = genderOptions.Select(x => x.Item1).ToList();

            _customGenderOptions.AddRange(genders);

            foreach(var option in genderOptions)
            {
                _customPronounMappings[option.Item1] = option.Item2;
            }
        }

        public void AddGenderOption(string genderOption, string pronouns)
        {
            _customGenderOptions.Add(genderOption);
            _customPronounMappings[genderOption] = pronouns;
        }

        public string[] GenderOptions
        {
            get
            {
                var options = _customGenderOptions.Any() == true
                                ? _customGenderOptions.ToArray()
                                : DefaultGenderChoices;

                return options;
            }
        }

        public string GetPronouns(string gender)
        {
            string pronouns = DefaultPronounMapping["Ambiguous"];
            if (_customPronounMappings.ContainsKey(gender))
            {
                pronouns = _customPronounMappings[gender];
            }
            else if(DefaultPronounMapping.ContainsKey(gender)) 
            {
                pronouns = DefaultPronounMapping[gender];
            }

            return pronouns;
        }
    }
}