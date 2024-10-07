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

namespace GameTools.TownsfolkManager.Tests.StepDefinitions
{
    

    [Binding]
    public class GenerateTownspersonStepDefinitions
    {
        
        private ITownsfolkManager _testClass;
		private Townsperson? _generatedNpc;

		public GenerateTownspersonStepDefinitions()
		{
			_testClass = ArrangeTownsfolkManager();
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

		[When("the user Requests an Npc with Elf as the species")]
		public void WhenTheUserRequestsAnNpcWithElfAsTheSpecies()
		{
			TownsfolkUserOptions options = new TownsfolkUserOptions();
			options.Species = "Elf";

			_generatedNpc = _testClass.GenerateTownspersonFromOptions(options);
		}

		[Then("the return value Species will be Elf")]
		public void ThenTheReturnValueSpeciesWillBeElf()
		{
			string actualSpecies = _generatedNpc?.Species ?? string.Empty;
			Assert.True((actualSpecies == "Elf"), $"The user selected an Elf NPC but got an {actualSpecies}");
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
