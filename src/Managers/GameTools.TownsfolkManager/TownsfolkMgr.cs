using GameTools.DiceEngine;
using GameTools.NPCAccess;
using GameTools.Ruleset.Definitions;
using GameTools.Ruleset.Definitions.Characters;
using GameTools.RulesetAccess.Contracts;
using GameTools.TownsfolkManager.Contracts;
using GameTools.TownsfolkManager.InternalOperations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ThatDeveloperDad.Framework.Wrappers;

namespace GameTools.TownsfolkManager
{

    public class TownsfolkMgr : ITownsfolkManager
    {
        
        private readonly IRulesetAccess _rulesAccess;
        private readonly INpcAccess _npcAccess;
        private readonly ICardDeck _shuffler;
        private readonly IDiceBag _diceBag;

        public TownsfolkMgr(IRulesetAccess ruleSet,
                            INpcAccess npcAccess,
                            ICardDeck shuffler,
                            IDiceBag diceBag)
        {
            _npcAccess = npcAccess;
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

        public Townsperson GenerateTownspersonFromOptions(TownsfolkUserOptions options)
        {
            Townsperson npc = new Townsperson();

            // IsRetired affects the age calculation that's run based on species.
            // We need to set that now.
			if (options.IsRetired.HasValue)
			{
				npc.Vocation.IsRetired = options.IsRetired.Value;
			}

			string selectedSpecies = options.Species??string.Empty;
            if(string.IsNullOrWhiteSpace(selectedSpecies))
            {
                selectedSpecies = _shuffler.PickOne(CharacterRules.SpeciesRules.List())??string.Empty;
            }
            SpeciesTemplate? speciesTplt = CharacterRules.SpeciesRules.Load(selectedSpecies);
            if(speciesTplt == null)
            {
                throw new Exception($"Could not apply species template for {selectedSpecies}.");
            }
            npc = npc.ApplySpecies(speciesTplt, _shuffler, _diceBag);

            string selectedBackground = options.Background ?? string.Empty;
            if(string.IsNullOrWhiteSpace(selectedBackground))
            {
                selectedBackground = _shuffler.PickOne(CharacterRules.BackgroundRules.List()) ?? string.Empty;
            }
            BackgroundTemplate? bgroundTplt = CharacterRules.BackgroundRules.Load(selectedBackground);
            if(bgroundTplt == null)
            {
                throw new Exception($"Could not apply background template for {selectedBackground}");
            }
            npc = npc.ApplyBackground(bgroundTplt, _shuffler, _diceBag);

            

            string selectedVocation = options.Vocation ?? string.Empty;
            if(string.IsNullOrWhiteSpace(selectedVocation))
            {
                selectedVocation = _shuffler.PickOne(CharacterRules.VocationRules.List()) ?? string.Empty;
            }
            VocationTemplate? jobTplt = CharacterRules.VocationRules.Load(selectedVocation);
            if(jobTplt == null)
            {
                throw new Exception($"Could not apply profession tempalte for {selectedVocation}");
            }
            npc = npc.ApplyVocation(jobTplt, _shuffler, _diceBag);

            return npc;
        }

        [Obsolete("This version is going away.  Use the TownsfolkUserOptions instead.")]
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

        public async Task<OpResult<IEnumerable<FilteredTownsperson>>> FilterTownspeople(TownspersonFilter filter)
        {
            List<FilteredTownsperson> managerPayload = new List<FilteredTownsperson>();
            OpResult<IEnumerable<FilteredTownsperson>> managerResult 
                = new OpResult<IEnumerable<FilteredTownsperson>>(managerPayload);

            
            NpcAccessFilter accessFilter = filter.ToNpcAccessFilter();
            
            var accessResult = await _npcAccess.FilterNpcs(accessFilter);

            if(accessResult != null && accessResult.WasSuccessful)
            {
                var filteredNpcs = accessResult.Payload ?? Array.Empty<NpcAccessFilterResult>();

                foreach(var npc in filteredNpcs)
                {
                    var mgrResult = npc.ToManagerModel();
                    managerPayload.Add(mgrResult);
                }
            }
            else
            {
                if(accessResult == null)
                {
                    managerResult.AddError(Guid.NewGuid(), "Something Very Bad happened.");
                }
                else
                {
                    foreach (var accessError in accessResult.Errors)
                    {
                        managerResult.AddError(accessError.Key, accessError.Value);
                    }
                }
            }

            return managerResult;
        }

        public async Task<OpResult<Townsperson>> LoadTownsperson(int npcId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<OpResult<Townsperson>> SaveTownsperson(Townsperson townsperson)
        {
            OpResult<Townsperson> managerResult = new OpResult<Townsperson>();
            managerResult.Payload = townsperson;

            //TODO:  Fix the transformer method to also map the Name.
            NpcAccessModel storageModel = townsperson.ToNpcAccessModel();

            OpResult<int> saveResult = await _npcAccess.SaveNpc(storageModel);

            if(saveResult.WasSuccessful)
            {
                managerResult.Payload.SetId(saveResult.Payload);
            }
            else
            {
                foreach (var item in saveResult.Errors)
                {
                    managerResult.AddError(item.Key, item.Value);
                }
            }

            return managerResult;
        }
    }
}