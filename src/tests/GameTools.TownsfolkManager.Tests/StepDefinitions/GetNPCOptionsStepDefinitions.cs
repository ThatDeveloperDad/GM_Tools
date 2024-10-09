using System;
using System.Collections.Generic;
using GameTools.DiceEngine;
using GameTools.NPCAccess;
using GameTools.Ruleset.Definitions;
using GameTools.Ruleset.Definitions.Characters;
using GameTools.RulesetAccess.Contracts;
using GameTools.TownsfolkManager.Contracts;
using GameTools.TownsfolkManager.Tests.Support;
using Moq;
using Reqnroll;
using Xunit;

namespace GameTools.TownsfolkManager.Tests.StepDefinitions
{
    [Binding]
    public class GetNPCOptionsStepDefinitions
    {

        private ITownsfolkManager? _testClass;
        private Dictionary<string, string[]>? _npcOptions;
        private ICardDeck _mockShuffler = new Mock<ICardDeck>().Object;
        private IDiceBag _mockDiceBag = new Mock<IDiceBag>().Object;
        private INpcAccess _mockNpcAccess = new Mock<INpcAccess>().Object;


        [BeforeScenario]
        public void TestSetup()
        {
            _npcOptions = null;
        }

        [Given("RulesetAccess is properly configured")]
        public void GivenRulesetAccessIsProperlyConfigured()
        {
            // When the Manager has a correct reference to the ResourceAccess
            // and it's been configured with a valid Ruleset
            // Our testClass is Properly Configured.

            // Can't Mock Abstract Classes.  Gonna have to use Fakes Instead
            // for these.
            Mock<IRulesetAccess> mockRulesetAccess = CreateMockRulesetAccess(3);

            _testClass = new TownsfolkMgr(mockRulesetAccess.Object,
                                              _mockNpcAccess,
                                              _mockShuffler,
                                              _mockDiceBag);

        }

        
        [When("I call GetNpcOptions")]
        public void WhenICallGetNpcOptions()
        {
            _npcOptions = _testClass?.GetNpcOptions();
        }

        [Then("I should receive a containing valid options for species, background, and vocation")]
        public void ThenIShouldReceiveAContainingValidOptionsForSpeciesBackgroundAndVocation()
        {
            string[]? speciesList = null;
            _npcOptions?.TryGetValue("Species", out speciesList);
            int? speciesCount = speciesList?.Length;

            string[]? vocationList = null;
            _npcOptions?.TryGetValue("Vocation", out vocationList);
            int? vocationCount = vocationList?.Length;

            string[]? backgroundList = null;
            _npcOptions?.TryGetValue("Background", out  backgroundList);
            int? backgroundCount = backgroundList?.Length;

            Assert.NotNull(speciesCount);
            Assert.True(speciesCount > 0);

            Assert.NotNull(backgroundCount);
            Assert.True(backgroundCount > 0);

            Assert.NotNull(vocationCount);
            Assert.True(vocationCount > 0);
        }

        [Given("Ruleset Access is not properly configured")]
        public void GivenRulesetAccessIsNotProperlyConfigured()
        {
            var mockRulesetAccess = CreateMockRulesetAccess(null);


            _testClass = new TownsfolkMgr(mockRulesetAccess.Object,
                                              _mockNpcAccess,
                                              _mockShuffler,
                                              _mockDiceBag);
        }

        [Then("GetNpcOptions throws An InvalidOperationException.")]
        public void ThenGetNpcOptionsThowsAnInvalidOperationException_()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass?.GetNpcOptions());
        }

        [Given("Ruleset Access is configured but the Ruleset is missing required template collections.")]
        public void GivenRulesetAccessIsConfiguredButTheRulesetIsMissingRequiredTemplateCollections_()
        {
            
            var mockRulesetAccess = CreateMockRulesetAccess(0);


            _testClass = new TownsfolkMgr(mockRulesetAccess.Object,
                                              _mockNpcAccess,
                                              _mockShuffler,
                                              _mockDiceBag);
        }

        [Then("I should receive a Dictionary with empty arrays for the missing option sets.")]
        public void ThenIShouldReceiveADictionaryWithEmptyArraysForTheMissingOptionSets_()
        {
            string[]? speciesList = null;
            _npcOptions?.TryGetValue("Species", out speciesList);
            int? speciesCount = speciesList?.Length;

            string[]? vocationList = null;
            _npcOptions?.TryGetValue("Vocation", out vocationList);
            int? vocationCount = vocationList?.Length;

            string[]? backgroundList = null;
            _npcOptions?.TryGetValue("Background", out backgroundList);
            int? backgroundCount = backgroundList?.Length;

            Assert.NotNull(speciesCount);
            Assert.True(speciesCount == 0);

            Assert.NotNull(backgroundCount);
            Assert.True(backgroundCount == 0);

            Assert.NotNull(vocationCount);
            Assert.True(vocationCount == 0);
        }

        private static Mock<IRulesetAccess> CreateMockRulesetAccess(int? templatesToAdd)
        {
            var mockRulesetAccess = new Mock<IRulesetAccess>();
            var mockRuleset = new Mock<ICharacterCreationRules>();

            if (templatesToAdd != null)
            {
                var mockSpeciesRules = RulesetFakes.CreatePopulatedSpeciesRules(templatesToAdd.GetValueOrDefault());
                var mockBackgroundRules = RulesetFakes.CreatePopulatedBackgrounds(templatesToAdd.GetValueOrDefault());
                var mockVocationRules = RulesetFakes.CreatePopulatedVocations(templatesToAdd.GetValueOrDefault());

                mockRuleset.Setup(x => x.SpeciesRules)
                           .Returns(mockSpeciesRules);
                mockRuleset.Setup(x => x.BackgroundRules)
                           .Returns(mockBackgroundRules);
                mockRuleset.Setup(x => x.VocationRules)
                           .Returns(mockVocationRules);
                mockRulesetAccess.Setup(x => x.LoadCharacterCreationRules())
                       .Returns(mockRuleset.Object);
            }
            else
            {
                mockRulesetAccess.Setup(x => x.LoadCharacterCreationRules())
                    .Throws(new InvalidOperationException("Ruleset has not been initialized."));
            }

            return mockRulesetAccess;
        }

    }
}
