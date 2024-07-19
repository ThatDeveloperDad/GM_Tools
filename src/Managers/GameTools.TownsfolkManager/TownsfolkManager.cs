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
                var speciesChoices = _npcRules.SpeciesRules.List();
                selectedSpecies = _shuffler.PickOne(speciesChoices);
            }
            SpeciesTemplate? speciesTemplate = _npcRules.SpeciesRules.Load(selectedSpecies);
            if (speciesTemplate != null)
            {
                npc.ApplySpecies(speciesTemplate);
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
                var professionChoices = _npcRules.VocationRules.List();
                selectedVocation = _shuffler.PickOne(professionChoices);
            }
            VocationTemplate? vocation = _npcRules.VocationRules.Load(selectedVocation);
            if (vocation != null)
            {
                npc.ApplyVocation(vocation);
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
                var backgroundChoices = _npcRules.BackgroundRules.List();
                selectedBackground = _shuffler.PickOne(backgroundChoices);
            }
            BackgroundTemplate? background = _npcRules.BackgroundRules.Load(selectedBackground);
            if(background != null)
            {
                npc.ApplyBackground(background);
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

            npcOptions.Add(nameof(Townsperson.Species), _npcRules.SpeciesRules.List());
            npcOptions.Add(nameof(Townsperson.Background), _npcRules.BackgroundRules.List());
            npcOptions.Add(nameof(Townsperson.Vocation), _npcRules.VocationRules.List());

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