using GameTools.Ruleset.Definitions;
using GameTools.Ruleset.Definitions.Characters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.TownsfolkManager.Tests.Support
{
    internal class RulesetFakes
    {
        public static SpeciesData CreatePopulatedSpeciesRules(int entryCount = 1)
        {
            SpeciesData fake = new FakeSpecies(entryCount);
            

            return fake;
        }
        public static BackgroundData CreatePopulatedBackgrounds(int entryCount = 1)
        {
            BackgroundData fake = new FakeBackgrounds(entryCount);
            return fake;
        }
        public static VocationData CreatePopulatedVocations(int entryCount = 1)
        {
            VocationData fake = new FakeVocations(entryCount);
            return fake;
        }
    }

    internal class FakeSpecies : SpeciesData
    {
        private int _entryCount;

        public FakeSpecies(int entryCount):base()
        {
            _entryCount = entryCount;
            InitializeTemplates();
        }

        protected override void InitializeTemplates()
        {
            if (_entryCount <= 0)
                return;

            for (int entryIndex = 1; entryIndex <= _entryCount; entryIndex++)
            {
                var tplt = new SpeciesTemplate($"species{entryIndex}");
                RegisterSpecies(tplt);
            }
        }
    }

    internal class FakeBackgrounds : BackgroundData
    {
        private int _entryCount;

        public FakeBackgrounds(int entryCount):base()
        {
            _entryCount= entryCount;
            InitializeTemplates();
        }

        protected override void InitializeTemplates()
        {
            if (_entryCount <= 0)
                return;

            for (int entryIndex = 1; entryIndex <= _entryCount; entryIndex++)
            {
                var tplt = new BackgroundTemplate($"background{entryIndex}");
                RegisterBackground( tplt );
            }
        }
    }

    internal class FakeVocations : VocationData
    {
        private int _entryCount;

        public FakeVocations(int entryCount):base()
        {
            _entryCount = entryCount;
            InitializeTemplates();
        }

        protected override void InitializeTemplates()
        {
            if (_entryCount <= 0)
                return;

            for (int entryIndex = 1; entryIndex <= _entryCount; entryIndex++)
            {
                var tplt = new VocationTemplate($"job{entryIndex}");
                RegisterVocation( tplt );
            }
        }
    }
}
