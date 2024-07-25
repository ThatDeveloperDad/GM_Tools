﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by Reqnroll (https://www.reqnroll.net/).
//      Reqnroll Version:2.0.0.0
//      Reqnroll Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace GameTools.TownsfolkManager.Tests.Features.TownsfolkManager
{
    using Reqnroll;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class GetNPCOptionsFeature : object, Xunit.IClassFixture<GetNPCOptionsFeature.FixtureData>, Xunit.IAsyncLifetime
    {
        
        private static global::Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "GetNpcOptions.feature"
#line hidden
        
        public GetNPCOptionsFeature(GetNPCOptionsFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
        }
        
        public static async System.Threading.Tasks.Task FeatureSetupAsync()
        {
            testRunner = global::Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(null, global::Reqnroll.xUnit.ReqnrollPlugin.XUnitParallelWorkerTracker.Instance.GetWorkerId());
            global::Reqnroll.FeatureInfo featureInfo = new global::Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features/TownsfolkManager", "GetNPCOptions", "Loads and returns the selectable character options from the \r\nRulesetAccess compo" +
                    "nent.", global::Reqnroll.ProgrammingLanguage.CSharp, featureTags);
            await testRunner.OnFeatureStartAsync(featureInfo);
        }
        
        public static async System.Threading.Tasks.Task FeatureTearDownAsync()
        {
            string testWorkerId = testRunner.TestWorkerId;
            await testRunner.OnFeatureEndAsync();
            testRunner = null;
            global::Reqnroll.xUnit.ReqnrollPlugin.XUnitParallelWorkerTracker.Instance.ReleaseWorker(testWorkerId);
        }
        
        public async System.Threading.Tasks.Task TestInitializeAsync()
        {
        }
        
        public async System.Threading.Tasks.Task TestTearDownAsync()
        {
            await testRunner.OnScenarioEndAsync();
        }
        
        public void ScenarioInitialize(global::Reqnroll.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public async System.Threading.Tasks.Task ScenarioStartAsync()
        {
            await testRunner.OnScenarioStartAsync();
        }
        
        public async System.Threading.Tasks.Task ScenarioCleanupAsync()
        {
            await testRunner.CollectScenarioErrorsAsync();
        }
        
        async System.Threading.Tasks.Task Xunit.IAsyncLifetime.InitializeAsync()
        {
            await this.TestInitializeAsync();
        }
        
        async System.Threading.Tasks.Task Xunit.IAsyncLifetime.DisposeAsync()
        {
            await this.TestTearDownAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Happy Path Scenario: Retrieving NPC Options Successfully")]
        [Xunit.TraitAttribute("FeatureTitle", "GetNPCOptions")]
        [Xunit.TraitAttribute("Description", "Happy Path Scenario: Retrieving NPC Options Successfully")]
        [Xunit.TraitAttribute("Category", "HappyPath")]
        public async System.Threading.Tasks.Task HappyPathScenarioRetrievingNPCOptionsSuccessfully()
        {
            string[] tagsOfScenario = new string[] {
                    "HappyPath"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Happy Path Scenario: Retrieving NPC Options Successfully", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 7
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 8
 await testRunner.GivenAsync("RulesetAccess is properly configured", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 9
 await testRunner.WhenAsync("I call GetNpcOptions", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 10
 await testRunner.ThenAsync("I should receive a containing valid options for species, background, and vocation" +
                        "", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="RulesetAccess is not configured", Skip="Ignored")]
        [Xunit.TraitAttribute("FeatureTitle", "GetNPCOptions")]
        [Xunit.TraitAttribute("Description", "RulesetAccess is not configured")]
        [Xunit.TraitAttribute("Category", "UnhappyPath")]
        public async System.Threading.Tasks.Task RulesetAccessIsNotConfigured()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore",
                    "UnhappyPath"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("RulesetAccess is not configured", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 15
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 16
 await testRunner.GivenAsync("Ruleset Access is not properly configured", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 17
 await testRunner.WhenAsync("I call GetNpcOptions", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 18
 await testRunner.ThenAsync("I should receive an empty dictionary", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Empty Lists of CharacterOptions in the Dictionary.")]
        [Xunit.TraitAttribute("FeatureTitle", "GetNPCOptions")]
        [Xunit.TraitAttribute("Description", "Empty Lists of CharacterOptions in the Dictionary.")]
        [Xunit.TraitAttribute("Category", "UnhappyPath")]
        public async System.Threading.Tasks.Task EmptyListsOfCharacterOptionsInTheDictionary_()
        {
            string[] tagsOfScenario = new string[] {
                    "UnhappyPath"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Empty Lists of CharacterOptions in the Dictionary.", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 24
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 25
 await testRunner.GivenAsync("Ruleset Access is configured but the Ruleset is missing required template collect" +
                        "ions.", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 26
 await testRunner.WhenAsync("I call GetNpcOptions", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 27
 await testRunner.ThenAsync("I should receive a Dictionary with empty arrays for the missing option sets.", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "2.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : object, Xunit.IAsyncLifetime
        {
            
            async System.Threading.Tasks.Task Xunit.IAsyncLifetime.InitializeAsync()
            {
                await GetNPCOptionsFeature.FeatureSetupAsync();
            }
            
            async System.Threading.Tasks.Task Xunit.IAsyncLifetime.DisposeAsync()
            {
                await GetNPCOptionsFeature.FeatureTearDownAsync();
            }
        }
    }
}
#pragma warning restore
#endregion
