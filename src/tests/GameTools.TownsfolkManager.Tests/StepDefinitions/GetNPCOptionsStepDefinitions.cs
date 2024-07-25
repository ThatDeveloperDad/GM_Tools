using System;
using System.Collections.Generic;
using GameTools.DiceEngine;
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

        private ITownsfolkManager _testClass;
        private Dictionary<string, string[]?>? _npcOptions;

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
            var mockSpeciesRules = RulesetFakes.CreatePopulatedSpeciesRules(3);

            var mockBackgroundRules = RulesetFakes.CreatePopulatedBackgrounds(4);

            var mockVocationRules = RulesetFakes.CreatePopulatedVocations(3);

            var mockRuleset = new Mock<ICharacterCreationRules>();
            mockRuleset.Setup(x => x.SpeciesRules)
                       .Returns(mockSpeciesRules);
            mockRuleset.Setup(x => x.BackgroundRules)
                       .Returns(mockBackgroundRules);
            mockRuleset.Setup(x => x.VocationRules)
                       .Returns(mockVocationRules);
            
            var mockRulesetAccess = new Mock<IRulesetAccess>();
            mockRulesetAccess.Setup(x => x.LoadCharacterCreationRules())
                       .Returns(mockRuleset.Object);

            var mockShuffler = new Mock<ICardDeck>();
            var mockDiceBag = new Mock<DiceBag>();

            _testClass = new TownsfolkManager(mockRulesetAccess.Object,
                                              mockShuffler.Object,
                                              mockDiceBag.Object);

        }

        [When("I call GetNpcOptions")]
        public void WhenICallGetNpcOptions()
        {
            _npcOptions = _testClass.GetNpcOptions();
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
            _npcOptions.TryGetValue("Background", out  backgroundList);
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
            throw new PendingStepException();
        }

        [Then("I should receive an empty dictionary")]
        public void ThenIShouldReceiveAnEmptyDictionary()
        {
            throw new PendingStepException();
        }

        [Given("Ruleset Access is configured but the Ruleset is missing required template collections.")]
        public void GivenRulesetAccessIsConfiguredButTheRulesetIsMissingRequiredTemplateCollections_()
        {
            var mockSpeciesRules = RulesetFakes.CreatePopulatedSpeciesRules(0);

            var mockBackgroundRules = RulesetFakes.CreatePopulatedBackgrounds(0);

            var mockVocationRules = RulesetFakes.CreatePopulatedVocations(0);

            var mockRuleset = new Mock<ICharacterCreationRules>();
            mockRuleset.Setup(x => x.SpeciesRules)
                       .Returns(mockSpeciesRules);
            mockRuleset.Setup(x => x.BackgroundRules)
                       .Returns(mockBackgroundRules);
            mockRuleset.Setup(x => x.VocationRules)
                       .Returns(mockVocationRules);

            var mockRulesetAccess = new Mock<IRulesetAccess>();
            mockRulesetAccess.Setup(x => x.LoadCharacterCreationRules())
                       .Returns(mockRuleset.Object);

            var mockShuffler = new Mock<ICardDeck>();
            var mockDiceBag = new Mock<DiceBag>();

            _testClass = new TownsfolkManager(mockRulesetAccess.Object,
                                              mockShuffler.Object,
                                              mockDiceBag.Object);
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
            _npcOptions.TryGetValue("Background", out backgroundList);
            int? backgroundCount = backgroundList?.Length;

            Assert.NotNull(speciesCount);
            Assert.True(speciesCount == 0);

            Assert.NotNull(backgroundCount);
            Assert.True(backgroundCount == 0);

            Assert.NotNull(vocationCount);
            Assert.True(vocationCount == 0);
        }
    }
}
