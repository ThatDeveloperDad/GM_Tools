using GameTools.Framework;
using GameTools.Framework.Concepts;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace GameTools.Ruleset.Definitions.Characters
{
    // Defines the properties that are common to the concept of a TTRPG Species.
    public class SpeciesTemplate
    {
        public readonly string[] DefaultGenderChoices = [ "Ambiguous", "Female", "Male" ];
        public readonly Dictionary<string, string> DefaultPronounMapping;
        private readonly List<string> _customGenderOptions;
        private readonly Dictionary<string, string> _customPronounMappings;
        public readonly Dictionary<AgeCategoryKind, int?> AgeMilestones;

        private readonly List<string> _integumentColors;
        private readonly List<string> _integumentStyles;
        private readonly List<string> _skinColors;

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

            AgeMilestones = new Dictionary<AgeCategoryKind, int?>
            {
                { AgeCategoryKind.Adulthood, null },
                { AgeCategoryKind.Retirement, null },
                { AgeCategoryKind.Lifespan, null }
            };

            _integumentColors = new List<string>();
            _integumentStyles = new List<string>();
            _skinColors = new List<string>();
        }
        
        public string Name { get; init; }

        #region Gender Options

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

        #endregion // Gender Options

        #region Height, Weight, and Age
        public int AverageHeightCm { get; set; }

        public int HeightVarianceCm { get; set; }

        public int AverageWeightKg { get; set; }

        public int WeightVarianceKg { get; set; }

        public void SetAgeMilestone(AgeCategoryKind ageCategory, int averageAge)
        {
            AgeMilestones[ageCategory] = averageAge;
        }

        #endregion

        #region Integument options

        public IntegumentKind IntegumentKind { get; set; }

        public string[] IntegumentColorOptions => _integumentColors.ToArray();

        public string[] IntegumentStyleOptions => _integumentStyles.ToArray();

        public string[] ComplexionColors => _skinColors.ToArray();

        public void AddIntegumentColor(string color)
        {
            _integumentColors.Add(color);
        }

        public void AddIntegumentColors(string[] colors)
        {
            _integumentColors.AddRange(colors);
        }

        public void AddIntegumentStyle(string style)
        {
            _integumentStyles.Add(style);
        }

        public void AddIntegumentStyles(string[] styles)
        {
            _integumentStyles.AddRange(styles);
        }

        public void AddComplexionColor(string color)
        {
            _skinColors.Add(color);
        }

        public void AddComplexionColors(string[] colors)
        {
            _skinColors.AddRange(colors);
        }

        #endregion
    }
}