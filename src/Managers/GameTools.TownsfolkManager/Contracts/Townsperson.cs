using GameTools.Framework;

namespace GameTools.TownsfolkManager.Contracts
{
    /// <summary>
    /// Describes a non-player-character in our game world.
    /// Includes properties that are applicable only to the people
    /// the party may meet on their travels that usually engage in
    /// non-combat interactions.
    /// </summary>
    public class Townsperson
    {

        public Townsperson()
        {
            GivenName = string.Empty;
            FamilyName = string.Empty;
            Species = string.Empty;
            Appearance = new CharacterAppearance();
            Vocation = new CharacterVocation();
            Background = new CharacterBackground();
            PersonalityNature = string.Empty;
            PersonalityDemeanor = string.Empty;
        }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string FullName => $"{GivenName} {FamilyName}";
        
        public int AgeYears { get; set; }

        public string Species { get; set; }

        public string? SubSpecies {get;set;}

        public CharacterAppearance Appearance {get;private set;}

        /// <summary>
        /// What does the character do?
        /// </summary>
        public CharacterVocation Vocation {get; private set;}

        /// <summary>
        /// How did they grow up?
        /// What impacts has this background had on their life, 
        /// worldview, and personality?
        /// </summary>
        public CharacterBackground Background {get; private set;}

        /// <summary>
        /// What is the character like in their inner world?
        /// i.e.:  Once you get to know them, what are they like?
        /// </summary>
        public string PersonalityNature {get;set;}

        /// <summary>
        /// How does the character present themselves to others?
        /// </summary>
        public string PersonalityDemeanor { get; set; }

    }

}
