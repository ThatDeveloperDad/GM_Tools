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
        int rollsPerTestRun = 10000;
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

#region RollModifier Tests

// When a roll result is modified, it is applied to the result of the dice roll.
[Fact]
public void DiceBag_WhenResultModifierIsNotZero_ItAltersTheResult()
{
    // Arrange  (Set up the experiment)
    IDiceBag testObject = new DiceBag();
    int numRocks = 3;
    MathRockKind rockKind = MathRockKind.D6;
    int resultModifier = -2;
    
    // Act  (Do the experiment)
    DiceTray diceRoll = testObject.Roll(numRocks, rockKind, resultModifier);

    string theoryStatement = $"The Result of the roll is adjusted by the modifer amount.";
    bool evidence = (diceRoll.ResultModifier != 0 && diceRoll.Result != diceRoll.UnadjustedResult)
                  ||(diceRoll.ResultModifier == 0 && diceRoll.Result == diceRoll.UnadjustedResult);

    // Assert (Analyze the results)
    Assert.True(evidence, theoryStatement);
}

// When a roll result is modified, it will never go below 0.
    [Fact]
    public void DiceBag_WhenResultModifierIsNotZero_TheResultWillNeverBeBelowZero()
    {
        // Arrange  (Set up the experiment)
    IDiceBag testObject = new DiceBag();
    int numRocks = 3;
    MathRockKind rockKind = MathRockKind.D6;
    int resultModifier = -20;
    
    // Act  (Do the experiment)
    DiceTray diceRoll = testObject.Roll(numRocks, rockKind, resultModifier);

    string theoryStatement = $"The Result of any adjusted roll must always be at least 0";
    bool evidence = (diceRoll.Result >= 0);

    // Assert (Analyze the results)
    Assert.True(evidence, theoryStatement);
    }

#endregion // RollModifier Tests

// ============================================================


}