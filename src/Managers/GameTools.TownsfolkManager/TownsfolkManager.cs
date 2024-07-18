using GameTools.DiceEngine;
using GameTools.Ruleset.Definitions.Characters;
using GameTools.RulesetAccess.Contracts;
using GameTools.TownsfolkManager.Contracts;
using System.Collections.Generic;

namespace GameTools.TownsfolkManager
{

    public class TownsfolkManager : ITownsfolkManager
    {
        private readonly ICharacterCreationRules _npcRules;
        private readonly ICardDeck _shuffler;
        private readonly IDiceBag _diceBag;

        public TownsfolkManager(IRulesetAccess ruleSet,
                                ICardDeck shuffler,
                                IDiceBag diceBag)
        {
            _npcRules = ruleSet.LoadCharacterCreationRules();
            _shuffler = shuffler;
            _diceBag = diceBag;
        }

        public Townsperson GenerateTownsperson()
        {
            Townsperson npc = new Townsperson();

            // Select the Species and add the available details.
            var speciesChoices = _npcRules.SpeciesRules.List();
            var selectedSpecies = _shuffler.PickOne(speciesChoices);
            npc.Species = selectedSpecies;

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

            // Select the Profession and add its details.
            var professionChoices = _npcRules.VocationRules.List();
            var npcProfession = _shuffler.PickOne(professionChoices);
            npc.Vocation.Name = npcProfession;
            
            // Does not follow from Species. May follow from Background. Probably not.
            //      Profession Name
            //      Titles
            //      Skills
            //      Length of Career

            // Select the Background and add the available details.
            var backgroundChoices = _npcRules.BackgroundRules.List();
            var npcBackground = _shuffler.PickOne(backgroundChoices);
            npc.Background.Name = npcBackground;

            //      Personality (Seeds)
            //      Skills / Proficiencies

            // Personality      (We're going to roll for KEYWORD attributes.)


            return npc;
        }

        public Dictionary<string, string[]> GetNpcOptions()
        {
            Dictionary<string, string[]> npcOptions = new Dictionary<string, string[]>();

            return npcOptions;
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