﻿using GameTools.Framework.Concepts;

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
            Pronouns = string.Empty;
            Species = string.Empty;
            Appearance = new CharacterAppearance();
            Vocation = new CharacterVocation();
            Background = new CharacterBackground();
            PersonalityNature = string.Empty;
            PersonalityDemeanor = string.Empty;
            FullName = new GeneratedProperty();
            PersonalityDescription = new GeneratedProperty();
        }

        //TODO:  Find a way to make sure these init; values don't get tampered with
        // while they're sitting in a client browser.
        public int? Id { get; init; }
        public int UserId { get; init; }
        public bool IsPublic { get; init; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string Pronouns { get; set; }

        public GeneratedProperty FullName { get; private set; }
        
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

        public GeneratedProperty PersonalityDescription { get; private set; }
    }

}
