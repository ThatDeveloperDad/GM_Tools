using System;
using GameTools.RulesetAccess;
using GameTools.RulesetAccess.Contracts;
using GameTools.Ruleset.DnD5eSRD;
using GameTools.TownsfolkManager.Contracts;
using Reqnroll;
using GameTools.NPCAccess;
using Moq;
using GameTools.DiceEngine;
using Xunit;
using Reqnroll.Assist.ValueRetrievers;
using Reqnroll.Assist;

namespace GameTools.TownsfolkManager.Tests.StepDefinitions
{
    

    [Binding]
    public class GenerateTownspersonStepDefinitions
    {
		private const string nullToken = "<null>";


        private ITownsfolkManager _testClass;
		private Townsperson? _generatedNpc;
		private TownsfolkUserOptions? _npcOptions;

		public GenerateTownspersonStepDefinitions()
		{
			_testClass = ArrangeTownsfolkManager();
		}

		[BeforeScenario]
		public void InitializeScenario()
		{
			_generatedNpc = null;
			_npcOptions = null;
		}

		private ITownsfolkManager ArrangeTownsfolkManager(bool isManagerValid = true)
        {
            if (isManagerValid)
            {
                return CreateValidTownsfolkManager();
            }
            else
            {
                return CreateInvalidTownsfolkManager();
            }
        }

		[Given("The TownsfolkManager is Ready to Generate NPCs")]
        public void GivenTheTownsfolkManagerIsReadyToGenerateNPCs()
        {
            _testClass = ArrangeTownsfolkManager();

			Assert.NotNull(_testClass);
        }

		[When("The user Requests an NPC without specifying options")]
		public void WhenTheUserRequestsAnNPCWithoutSpecifyingOptions()
		{
			_generatedNpc = _testClass.GenerateTownsperson();
		}

		[When("the user supplies attributes with the NPC request")]
		public void WhenTheUserSuppliesAttributesWithTheNPCRequest()
		{
			_npcOptions = new TownsfolkUserOptions();
		}
		
		[When("the requested Species is {string}")]
		public void WhenTheRequestedSpeciesIs(string? species)
		{
			if(species == nullToken)
			{
				species = null;
			}
			_npcOptions = _npcOptions ?? new TownsfolkUserOptions();
			_npcOptions.Species = species;
		}

		[When("the options are sent to the TownsfolkManager")]
		public void WhenTheOptionsAreSentToTheTownsfolkManager()
		{
			_npcOptions = _npcOptions??new TownsfolkUserOptions();
			_generatedNpc = _testClass.GenerateTownspersonFromOptions(_npcOptions);
		}
		
		[When("the requested Background is {string}")]
		public void WhenTheRequestedBackgroundIs(string? background)
		{
			if(background == nullToken)
			{
				background = null;
			}
			_npcOptions = _npcOptions ?? new TownsfolkUserOptions();
			_npcOptions.Background = background;

		}

		[When("the requested Vocation is {string}")]
		public void WhenTheRequestedVocationIs(string? vocation)
		{
			if(vocation == nullToken)
			{
				vocation = null;
			}

			_npcOptions = _npcOptions ?? new TownsfolkUserOptions();
			_npcOptions.Vocation = vocation;
		}

		[Then("the Townsperson will have their Background set to {string}")]
		public void ThenTheTownspersonWillHaveTheirBackgroundSetTo(string expectedBackground)
		{
			string actualBackground = _generatedNpc?.Background.Name ?? string.Empty;
			if (expectedBackground != nullToken)
			{
				Assert.Equal(expectedBackground, actualBackground);
			}
			else
			{
				Assert.False(string.IsNullOrWhiteSpace(actualBackground));
			}
		}

		[Then("the Townsperson will have their Vocation set to {string}")]
		public void ThenTheTownspersonWillHaveTheirVocationSetTo(string expectedVocation)
		{
			string actualVocation = _generatedNpc?.Vocation.Name ?? string.Empty;
			if (expectedVocation != nullToken)
			{
				Assert.StartsWith(expectedVocation, actualVocation);
			}
			else
			{
				Assert.False(string.IsNullOrWhiteSpace(actualVocation));
			}
		}

		[Then("the Townsperson will have their Species set to {string}")]
		public void ThenTheTownspersonWillHaveTheirSpeciesSetTo(string expectedSpecies)
		{
			string actualSpecies = _generatedNpc?.Species ?? string.Empty;
			if (expectedSpecies != nullToken)
			{
				Assert.Equal(expectedSpecies, actualSpecies);
			}
			else
			{
				Assert.False(string.IsNullOrWhiteSpace(actualSpecies));
			}
		}

		[Then("The GenerateTownsperson Method will return a non-null value")]
		public void ThenTheGenerateTownspersonMethodWillReturnANon_NullValue()
		{
			Assert.NotNull(_generatedNpc);
		}

		[Then("the return value will be a Townsperson instance")]
		public void ThenTheReturnValueWillBeATownspersonInstance()
		{
			Assert.IsType<Townsperson>(_generatedNpc);
		}


		[Then("the return value will have a Species")]
		public void ThenTheReturnValueWillHaveASpecies()
		{
			Assert.False(string.IsNullOrWhiteSpace(_generatedNpc?.Species), 
						 "The Species was not assigned.");
		}

		[Then("the return value will have a Vocation")]
		public void ThenTheReturnValueWillHaveAVocation()
		{
			Assert.False(string.IsNullOrWhiteSpace(_generatedNpc?.Vocation.Name),
						 "The Vocation was not assigned.");
		}

		[Then("the return value will have a Background")]
		public void ThenTheReturnValueWillHaveABackground()
		{
			Assert.False(string.IsNullOrWhiteSpace(_generatedNpc?.Background.Name),
						 "The Background was not assigned.");
		}




		#region Private Test Utility Methods

		private ITownsfolkManager CreateValidTownsfolkManager()
        {
			IRulesetAccess ruleSet = new RulesetProvider().Use5eSRD();
			INpcAccess npcAccess = new Mock<INpcAccess>().Object;
			ICardDeck shuffler = new DeckSimulator();
			IDiceBag diceBag = new DiceBag();


			TownsfolkMgr manager = new TownsfolkMgr(ruleSet,
													npcAccess,
													shuffler,
													diceBag);
			return manager;
		}

        private ITownsfolkManager CreateInvalidTownsfolkManager()
        {
			IRulesetAccess ruleSet = new Mock<IRulesetAccess>().Object;
			INpcAccess npcAccess = new Mock<INpcAccess>().Object;
			ICardDeck shuffler = new Mock<ICardDeck>().Object;
			IDiceBag diceBag = new Mock<IDiceBag>().Object;


			TownsfolkMgr manager = new TownsfolkMgr(ruleSet,
													npcAccess,
													shuffler,
													diceBag);
			return manager;
		}

        

		#endregion
	}
}
