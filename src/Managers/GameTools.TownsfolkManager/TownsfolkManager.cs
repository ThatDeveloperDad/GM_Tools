using GameTools.DiceEngine;
using GameTools.Ruleset.Definitions;
using GameTools.Ruleset.Definitions.Characters;
using GameTools.RulesetAccess.Contracts;
using GameTools.TownsfolkManager.Contracts;
using GameTools.TownsfolkManager.InternalOperations;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameTools.TownsfolkManager
{

    public class TownsfolkManager : ITownsfolkManager
    {
        
        private readonly IRulesetAccess _rulesAccess;
        private readonly ICardDeck _shuffler;
        private readonly IDiceBag _diceBag;

        public TownsfolkManager(IRulesetAccess ruleSet,
                                ICardDeck shuffler,
                                IDiceBag diceBag)
        {
            _rulesAccess = ruleSet;
            _shuffler = shuffler;
            _diceBag = diceBag;
        }

        private ICharacterCreationRules CharacterRules
        {
            get
            {
                return _rulesAccess.LoadCharacterCreationRules();
            }
        }

        public Townsperson GenerateTownsperson()
        {
            Townsperson npc = new Townsperson();

            

            var selectedOptions = new Dictionary<string, string?>();
            foreach (var entry in GetNpcOptions())
            {
                selectedOptions.Add(entry.Key, null);
            }

            // Select the Species and add the available details.
            var speciesChoices = CharacterRules.SpeciesRules.List();
            var selectedSpecies = _shuffler.PickOne(speciesChoices);
            selectedOptions[nameof(Townsperson.Species)] = selectedSpecies;

            // Name, Subspecies, and Appearance all derive from the Species.

            // Species
            //      SubSpecies
            //      Naming Lists
            //      Appearance
            //          Eye Color
            //          Eye Count
            //          Body Style

            // Select the Profession and add its details.
            var professionChoices = CharacterRules.VocationRules.List();
            var npcProfession = _shuffler.PickOne(professionChoices);
            selectedOptions[nameof(Townsperson.Vocation)] = npcProfession;
            
            // Does not follow from Species. May follow from Background. Probably not.
            //      Profession Name
            //      Titles
            //      Skills
            //      Length of Career

            // Select the Background and add the available details.
            var backgroundChoices = CharacterRules.BackgroundRules.List();
            var npcBackground = _shuffler.PickOne(backgroundChoices);
            selectedOptions[nameof(Townsperson.Background)] = npcBackground;

            //      Personality (Seeds)
            //      Skills / Proficiencies

            // Personality      (We're going to roll for KEYWORD attributes.)

            npc = GenerateTownspersonFromOptions(selectedOptions);

            return npc;
        }

        public Townsperson GenerateTownspersonFromOptions(Dictionary<string, string?> selectedAttributes)
        {
            Townsperson npc = new Townsperson();

            // Select the Species and add the available details.
            string selectedSpecies;
            if (selectedAttributes[nameof(Townsperson.Species)] != null)
            {
                selectedSpecies = selectedAttributes[nameof(Townsperson.Species)]!;
            }
            else
            {
                var speciesChoices = CharacterRules.SpeciesRules.List();
                selectedSpecies = _shuffler.PickOne(speciesChoices);
            }
            SpeciesTemplate? speciesTemplate = CharacterRules.SpeciesRules.Load(selectedSpecies);
            if (speciesTemplate != null)
            {
                npc.ApplySpecies(speciesTemplate, _shuffler, _diceBag);
            }
            else
            {
                throw new Exception("Something very dumb happened.");
            }

            //Vocations
            string selectedVocation;
            if (selectedAttributes[nameof(Townsperson.Vocation)] != null)
            {
                selectedVocation = selectedAttributes[nameof(Townsperson.Vocation)]!;
            }
            else
            {
                // Select the Profession and add its details.
                var professionChoices = CharacterRules.VocationRules.List();
                selectedVocation = _shuffler.PickOne(professionChoices);
            }
            VocationTemplate? vocation = CharacterRules.VocationRules.Load(selectedVocation);
            if (vocation != null)
            {
                npc.ApplyVocation(vocation, _shuffler, _diceBag);
            }
            else
            {
                throw new Exception("Something else stupid happened.");
            }

            string selectedBackground;
            if (selectedAttributes[nameof(Townsperson.Background)] != null)
            {
                selectedBackground = selectedAttributes[nameof(Townsperson.Background)]!;
            }
            else
            {
                // Select the Background and add the available details.
                var backgroundChoices = CharacterRules.BackgroundRules.List();
                selectedBackground = _shuffler.PickOne(backgroundChoices);
            }
            BackgroundTemplate? background = CharacterRules.BackgroundRules.Load(selectedBackground);
            if(background != null)
            {
                npc.ApplyBackground(background, _shuffler, _diceBag);
            }
            else
            {
                throw new Exception("A different bad thing happened.");
            }

            return npc;
        }

        public Dictionary<string, string[]> GetNpcOptions()
        {
            Dictionary<string, string[]> npcOptions = new Dictionary<string, string[]>();

            npcOptions.Add(nameof(Townsperson.Species), CharacterRules.SpeciesRules.List());
            npcOptions.Add(nameof(Townsperson.Background), CharacterRules.BackgroundRules.List());
            npcOptions.Add(nameof(Townsperson.Vocation), CharacterRules.VocationRules.List());

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