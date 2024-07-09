using GameTools.TownsfolkManager.Contracts;

namespace GameTools.TownsfolkManager
{

    public class TownsfolkManager : ITownsfolkManager
    {
        public Townsperson GenerateTownsperson()
        {
            Townsperson generatedNPC = new Townsperson();

            // QUESTION:  What do we need to know about a TownsPerson?

            // Species
            //      SubSpecies
            //      Naming Lists
            //      Appearance
            //          Height   (ranged variance from base height)
            //          Weight   (ranged variance from base weight)
            //          Age
            //          Gender
            //          Hair Color
            //          Hair Style
            //          Eye Color
            //          Eye Count
            //          Body Style

            // Profession
            // Does not follow from Species. May follow from Background. Probably not.
            //      Profession Name
            //      Titles
            //      Skills
            //      Length of Career

            // Background       (Selected from a list of choices)
            //      Background Name
            //      Personality (Seeds)
            //      Skills / Proficiencies

            // Personality      (We're going to roll for KEYWORD attributes.)



            
            // *****  Everything below here is NOT Minimum Viable Product. *****

            // **GameStats**  (Should this be defined within a specific implementation for the Ruleset concept?)

            // "Adventuring Class"
            // Not all NPCs will have this.  Some will, most won't.
            // If Present, will affect:  Game Stats, Skills, Proficiencies

            // Personality Details
            //      Informed by Background.  
            //      (We are FORMED BY where we came from, but not CONSTRAINED BY that.)
            //      Affected by life events.
            //      Each character has:
            //          Nature  (What they're like in private)
            //          Demeanor (What they're like in public)

            return generatedNPC;
        }

        public Townsperson[] ListTownspersons()
        {
            throw new System.NotImplementedException();
        }

        public Townsperson LoadTownsperson(string location)
        {
            throw new System.NotImplementedException();
        }

        public string SaveTownsperson(Townsperson townsperson)
        {
            throw new System.NotImplementedException();
        }
    }
}