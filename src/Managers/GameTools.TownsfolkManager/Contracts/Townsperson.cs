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
        }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string FullName => $"{GivenName} {FamilyName}";
        
        public string Species { get; set; }

        public string? SubSpecies {get;set;}

        public CharacterAppearance Appearance {get;private set;}

    }

}
