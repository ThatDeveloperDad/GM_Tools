using GameTools.Ruleset.Definitions.Characters;
using GameTools.RulesetAccess.Contracts;
using GameTools.TownsfolkManager.Contracts;

namespace GameTools.TownsfolkManager
{

    public class TownsfolkManager : ITownsfolkManager
    {
        private readonly ICharacterCreationRules _npcRules;

        public TownsfolkManager(IRulesetAccess ruleSet)
        {
            _npcRules = ruleSet.LoadCharacterCreationRules();
        }

        public Townsperson GenerateTownsperson()
        {
            Townsperson npc = new Townsperson();

            // Assign the Species.
            // Get the list of Species we can choose from.
            // Pick one of those.
            // How can we choose a Species?
            //   Pass in its name, selected from a list.
            //   Count the available choices, and generate a random number and pick that choice.
            //   Some other way.
            // Assign it to the NPC's Species property.
            npc.Species = "Elf";

            // Name, Subspecies, and Appearance all derive from the Species.

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


            return npc;
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