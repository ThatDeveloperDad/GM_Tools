using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameTools.DiceEngine.Tests;

public class DiceBagNoModifierTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        List<object[]> testCases = new List<object[]>();

        // roll 1 through 10 of each kind of Mathrock, and make sure nothing fails.
        int testsPerKind = 10;
        int rollsPerTestRun = 1000;
        var mathRockKinds = Enum.GetValues<MathRockKind>();

        foreach (MathRockKind mathRock in mathRockKinds)
        {
            for(int numDice = 1; numDice <= testsPerKind; numDice++)
            {
                testCases.Add(new object[] {numDice, mathRock, rollsPerTestRun});
            }
        }

        return testCases.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
}

public class DiceBagAdjustResultTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        List<object[]> testCases = new List<object[]>();

        // For each of those tests, we want to test with a result modifier of both positive and
        // negative values from [-5 .. 5]
        // Finally, for each combination of parameters, we want a number of executions.
        // (Because we're dealing with random numbers, and we want to make sure we're not just getting
        // "lucky" on the executions that run.)
        //
        // Our object[] will look like this: { numberOfRocks, rockKind, modifier, numberOfRuns }
        var mathRockKinds = Enum.GetValues<MathRockKind>();
        var rockCountCases = Enumerable.Range(1, 5).ToArray();        
        var adjustmentCases = Enumerable.Range(-5, 11).ToArray();
        int runsPerTest = 100; 
        // Keep runsPerTest reasonable, because we're currently at:
        // 8 * 5 * 11 * [whatever we set here.]
        // 440 * 100 = 44,000 executions / Test Method * 2 Methods = 880,000 tests.
        // Unit tests should be FAST!

        // For each kind of Mathrock, we want a test where we roll [1, 2, 3, 4, 5]
        foreach(MathRockKind mathRock in mathRockKinds)
        {
            foreach(int numberOfRocks in rockCountCases)
            {
                foreach(int rollAdjustment in adjustmentCases)
                {
                    testCases.Add(new object[]{numberOfRocks, mathRock, rollAdjustment, runsPerTest});
                }
            }
        }
    
        return testCases.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
}

public class DiceBagTests
{
    /// <summary>
    /// Our DiceBag exists, and is ready to use.
    /// </summary>
    [Fact]
    public void DiceBag_Exists()
    {
        IDiceBag? testObject = new DiceBag();

        Assert.NotNull(testObject);
    }
#region Basic Roll Tests
    /// <summary>
    /// The result is greater than zero
    /// </summary>
    [Theory]
    [ClassData(typeof(DiceBagNoModifierTestData))]
    public void DiceBag_UnmodifiedRoll_Expect_Positive(int numberOfDice, MathRockKind mathRock, int rollsPerTestRun = 1)
    {
        // Arrange (Get all the stuff for our experiment.)
        IDiceBag testObject = new DiceBag();
        List<DiceTray> testRun = new List<DiceTray>();

        // Act  (Do the experiemnt)
        for(int run = 0; run < rollsPerTestRun; run++)
        {
            var diceTray = testObject.Roll(numberOfDice, mathRock);
            testRun.Add(diceTray);
        }
        
        string theoryStatement = "All diceTrays in our test batch have a positive, non-zero value:";
        bool evidence = testRun.All(r=> r.Result>0);
        
        // Assert (Check our results!)
        Assert.True(evidence, theoryStatement);
        
    }

    // The result of the unmodified Roll is at least the number of dice we rolled.
    [Theory]
    [ClassData(typeof(DiceBagNoModifierTestData))]
    public void DiceBag_Roll_N_Dice_UnmodifiedRollMustBe_AtLeastN
        (int numberOfDice, MathRockKind mathRock, int rollsPerTestRun = 1)
    {
        // Arrange (Get all the stuff for our experiment.)
        IDiceBag testObject = new DiceBag();
        List<DiceTray> testRun = new List<DiceTray>();

        // Act  (Do the experiemnt)
        for(int run = 0; run < rollsPerTestRun; run++)
        {
            var diceTray = testObject.Roll(numberOfDice, mathRock);
            testRun.Add(diceTray);
        }
        
        string theoryStatement = "All diceTrays in our test batch have a result equal or greater than the number of dice:";
        bool evidence = testRun.All(r=> r.Result >= numberOfDice);
        
        // Assert (Check our results!)
        Assert.True(evidence, theoryStatement);
    }

    // The result of the unmodified Roll is no more than NumberOfDice x DiceKind.
    [Theory]
    [ClassData(typeof(DiceBagNoModifierTestData))]
    public void DiceBag_Roll_N_Dice_UnmodifiedRollMustBe_NoMoreThan_NumXSides
        (int numberOfDice, MathRockKind mathRock, int rollsPerTestRun = 1)
    {
        // Arrange (Get all the stuff for our experiment.)
        IDiceBag testObject = new DiceBag();
        int expected = numberOfDice*(int)mathRock;
        List<DiceTray> testRun = new List<DiceTray>();

        // Act  (Do the experiemnt)
        for(int run = 0; run < rollsPerTestRun; run++)
        {
            var diceTray = testObject.Roll(numberOfDice, mathRock);
            testRun.Add(diceTray);
        }
        
        string theoryStatement = $"All diceTrays in our test batch must have a result at or below {expected}:";
        bool evidence = testRun.All(r=> r.Result<=expected);
        
        // Assert (Check our results!)
        Assert.True(evidence, theoryStatement);
        
    }

    // We rolled the correct number of dice.
    [Theory]
    [ClassData(typeof(DiceBagNoModifierTestData))]
    public void DiceBag_ExpectDiceRolled_Equals_NumDice
        (int diceCount, MathRockKind mathRockKind, int numRunsInTest = 1)
    {
        // We're not going to use the numRunsInTest.  Since we're counting
        // the number of dice rolled = expected, we don't need to worry about
        // random numbers affecting this assement of the operation.
        // I'm assigning it to a dummy var to make the compiler warning go away.
        var dummyVariable = numRunsInTest;
        // Arrange (Get all the stuff for our experiment.)
        IDiceBag testObject = new DiceBag();
        int expected = diceCount;

        // Act  (Do the experiemnt)
        var diceTray = testObject.Roll(diceCount, mathRockKind);

        //Assert
        Assert.Equal(expected, diceTray.RollCount);
    }
#endregion // Basic roll tests

// ============================================================

#region RollAdjustment Tests

// When a roll result is modified, it is applied to the result of the dice roll.
[Theory]
[ClassData(typeof(DiceBagAdjustResultTestData))]
public void DiceBag_WhenResultModifierIsNotZero_ItAltersTheResult
    (int numberOfRocks, MathRockKind mathRockKind, int resultAdjustment, int numberOfRuns)
{
    // Arrange  (Set up the experiment)
    string theoryStatement = $"The Result of the roll is adjusted by the modifer amount.";
    var evidenceFunction = new Func<DiceTray, bool>((diceRoll)=>
        { 
            bool passed = (diceRoll.RollAdjustment != 0 && diceRoll.Result != diceRoll.UnadjustedResult)
                  ||(diceRoll.RollAdjustment == 0 && diceRoll.Result == diceRoll.UnadjustedResult);

            return passed;
        });

    IDiceBag testObject = new DiceBag();
    List<DiceTray> testRuns = new List<DiceTray>();

    // Act  (Do the experiment)
    for(int testIndex=0;testIndex<numberOfRuns;testIndex++)
    {
        DiceTray diceRoll = testObject.Roll(numberOfRocks, mathRockKind, resultAdjustment);
        testRuns.Add(diceRoll);
    }
    
    bool evidence = testRuns.All(evidenceFunction);

    // Assert (Analyze the results)
    Assert.True(evidence, theoryStatement);
}

    // When a roll result is modified, it will never go below 0.
    [Theory]
    [ClassData(typeof(DiceBagAdjustResultTestData))]
    public void DiceBagTests_TestAdjustedRolls_NeverNegativeResults
        (int numberOfRocks, MathRockKind mathRockKind, int resultAdjustment, int numberOfRuns)
    {
        // Arrange  (Set up the experiment)
        string theoryStatement = $"The Result of any adjusted roll must always be at least 0";
        var evidenceFunction = new Func<DiceTray, bool>((diceRoll)=> diceRoll.Result>=0);

        IDiceBag testObject = new DiceBag();
        List<DiceTray> testRuns = new List<DiceTray>();
    
        // Act  (Do the experiment)
        for(int testRun = 0;testRun<numberOfRuns;testRun++)
        {
            DiceTray diceRoll = testObject.Roll(numberOfRocks, mathRockKind, resultAdjustment);
            testRuns.Add(diceRoll);
        }
        
        bool evidence = testRuns.All(evidenceFunction);

        // Assert (Analyze the results)
        Assert.True(evidence, theoryStatement);
    }

#endregion // RollModifier Tests

// ============================================================

#region RollModifier Tests

    // When we request that the Roll be modified, 
    //the number of dice to be rolled should be one higher than requested.
    [Theory]
    [ClassData(typeof(DiceBagNoModifierTestData))]
    public void DiceBagTests_ModifiedRolls_HaveOneExtraMathRock
        // These pragma warnings were added after much thought.
        // To stand up a whole new ClassData type to be able to remove an
        // unused parameter seemed silly and needless.
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
        (int numberOfDice, MathRockKind rockKind, int numberOfRuns)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
#pragma warning restore IDE0060 // Remove unused parameter
    {
        // We're ignoring the numberOfRuns here, because we don't
        // care about the RESULTS of the dice being rolled,
        // We care only that the correct count of dice were rolled.

        string theoryStatement = "Modifying our roll adds an extra MathRock to the tray.";
        // Arrange
        IDiceBag testObject = new DiceBag();
        int rollAdjustment = 0;
        RollModifierKind modifier = RollModifierKind.Advantage;

        int expectedDiceInTray = numberOfDice + 1;

        // Act
        DiceTray tray = testObject.Roll(numberOfDice, rockKind, rollAdjustment, modifier);

        int diceInTray = tray.RollCount;

        bool evidence = expectedDiceInTray == diceInTray;

        // Assert
        Assert.True(evidence, theoryStatement);
    }

    // When we make a modified roll,  The result must be between 1 and the number of
    // sides on our chosen mathrock, multiplied by the number of mathRocks we requested.
    [Theory]
    [ClassData(typeof(DiceBagNoModifierTestData))]
    public void DiceBagTest_ModifiedRoll_ResultIsInCorrectRange
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
        (int numberOfDice, MathRockKind rockKind, int numberOfRuns)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
#pragma warning restore IDE0060 // Remove unused parameter
    {
        // Ignoring numberOfRuns here too.

        string theoryStatement = "When modifying the roll, our result will exclude the extra mathrock.";
        // Arrange
        IDiceBag testObject = new DiceBag();
        int rollAdjustment = 0;
        RollModifierKind modifier = RollModifierKind.Advantage;

        int startOfRange = (numberOfDice + rollAdjustment)<0        // Condition
                            ? 0                                     // If True
                                : numberOfDice + rollAdjustment;    // If False
        int endOfRange = (numberOfDice * (int)rockKind) + rollAdjustment;

        // Act
        DiceTray tray = testObject.Roll(numberOfDice, rockKind, rollAdjustment, modifier);

        bool evidence = (tray.Result >= startOfRange)
                        && (tray.Result <= endOfRange);

        // Assert
        Assert.True(evidence, theoryStatement);
    }

    // When we roll with Advantage, the math rock with the lowest
    // value is not included in the result.
    [Theory]
    [ClassData(typeof(DiceBagNoModifierTestData))]
    public void DiceBagTest_RollWithAdvantage_LowestResultIsIgnored
        (int numberOfDice, MathRockKind rockKind, int numberOfRuns)
    {
        // numberOfRuns isn't really needed for this test.
        // We're going to override it to 1.
        numberOfRuns = 1;

        string theoryStatement = "When rolling with Advantage, the lowest Math Rock in the tray is ignored.";
        Func<DiceTray, bool> evidenceTest = (tray)=>
        {
            bool lowestIsDiscarded = false;
            
            // We have a problem here.  I need the RolledMathRock, not just the value...
            int lowestRollValue = tray.AllRolls.OrderBy(r => r.Value).First().Value;
            RolledMathRock? discardedMathRock = tray.AllRolls.FirstOrDefault(r=> r.IsDiscarded == true);

            if(discardedMathRock == null)
            {
                return false;
            }

            lowestIsDiscarded = (discardedMathRock.Value == lowestRollValue);

            return lowestIsDiscarded;
        };

        // Arrange
        IDiceBag testObject = new DiceBag();
        int rollAdjustment = 0;
        RollModifierKind modifier = RollModifierKind.Advantage;
        List<DiceTray> experimentRuns = new List<DiceTray>();

        // Act
        for(int run = 0;run<numberOfRuns;run++)
        {
            DiceTray tray = testObject.Roll(numberOfDice, rockKind, rollAdjustment, modifier);
            experimentRuns.Add(tray);
        }

        bool evidence = experimentRuns.All(er=> evidenceTest(er) == true);

        // Assert
        Assert.True(evidence, theoryStatement);
    }

    /// <summary>
    /// When we roll with Disadvantage, the math rock with the highest value is excluded from the result.
    /// 
    /// Note:  We aren't using a test data provider that varies the RollModifier or Adjustment factors.
    /// We're interested only in making sure Disadvantage is working correctly.
    /// </summary>
    /// <param name="numberOfDice">How many dice to roll?</param>
    /// <param name="rockKind">What Kind of DIce?</param>
    /// <param name="numberOfRuns">How many experiments to run for this test?</param>
    [Theory]
    [ClassData(typeof(DiceBagNoModifierTestData))]
    public void DiceBagTest_RollWithDisadvantage_HighestResultIsIgnored
        (int numberOfDice, MathRockKind rockKind, int numberOfRuns)
    {
        // numberOfRuns isn't really needed for this test.
        // We're going to override it to 1.
        numberOfRuns = 1;

        string theoryStatement = "When rolling with Disadvantage, the Highest Math Rock in the tray is ignored.";

        Func<DiceTray, bool> evidenceFunction = (tray) => {
            bool highestWasDiscarded = false;
        
            // We have a problem here.  I need the RolledMathRock, not just the value...
            int highestRollValue = tray.AllRolls.OrderByDescending(r=> r.Value).First().Value;
            RolledMathRock? discardedRock = tray.AllRolls.FirstOrDefault(r=> r.IsDiscarded == true);
            
            if(discardedRock == null)
            {
                return false;
            }

            // If the value of the discarded Rock == highest roll value, we're good.
            highestWasDiscarded = (discardedRock.Value == highestRollValue);

            return highestWasDiscarded;
        };


        // Arrange
        IDiceBag testObject = new DiceBag();
        int rollAdjustment = 0;
        RollModifierKind modifier = RollModifierKind.Disadvantage;

        List<DiceTray> experimentRuns = new List<DiceTray>();

        // Act
        for(int run = 0; run < numberOfRuns; run++)
        {
            DiceTray tray = testObject.Roll(numberOfDice, rockKind, rollAdjustment, modifier);
            experimentRuns.Add(tray);
        }

        bool evidence = experimentRuns.All(er=> evidenceFunction(er) == true);

        // Assert
        Assert.True(evidence, theoryStatement);
    }
#endregion // RollModifier Tests

}