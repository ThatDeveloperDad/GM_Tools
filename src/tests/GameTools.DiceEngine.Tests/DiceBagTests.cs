namespace GameTools.DiceEngine.Tests;

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

    /// <summary>
    /// The result is greater than zero
    /// </summary>
    [Fact]
    public void DiceBag_Roll_1_MathRock_Expect_Positive()
    {
        // Arrange (Get all the stuff for our experiment.)
        IDiceBag testObject = new DiceBag();
        int numberOfDice = 1;
        MathRockKind mathRock = MathRockKind.D2;
        // Act  (Do the experiemnt)
        var diceTray = testObject.Roll(numberOfDice, mathRock);
        
        // Assert (Check our results!)
        Assert.True(diceTray.Result > 0);
    }

    // The result of the unmodified Roll is at least the number of dice we rolled.
    [Fact]
    public void DiceBag_Roll_N_Dice_RollMustBe_AtLeastN()
    {
        // Arrange (Get all the stuff for our experiment.)
        IDiceBag testObject = new DiceBag();
        int numberOfDice = 6;
        MathRockKind mathRock = MathRockKind.D2;
        // Act  (Do the experiemnt)
        var diceTray = testObject.Roll(numberOfDice, mathRock);

        // Assert (Check our results!)
        Assert.True(diceTray.Result >= numberOfDice);
    }

    // The result of the unmodified Roll is no more than NumberOfDice x DiceKind.
    [Fact]
    public void DiceBag_Roll_N_Dice_RollMustBe_NoMoreThan_NumXSides()
    {
        // Arrange (Get all the stuff for our experiment.)
        IDiceBag testObject = new DiceBag();
        int numberOfDice = 6;
        MathRockKind mathRock = MathRockKind.D2;

        int expected = numberOfDice*(int)mathRock;

        // Act  (Do the experiemnt)
        var diceTray = testObject.Roll(numberOfDice, mathRock);

        // Assert
        Assert.True(diceTray.Result <= expected);
        
    }

    // We rolled the correct number of dice.
    [Fact]
    public void DiceBag_ExpectDiceRolled_Equals_NumDice()
    {
        // Arrange (Get all the stuff for our experiment.)
        IDiceBag testObject = new DiceBag();
        int numberOfDice = 6;
        MathRockKind mathRock = MathRockKind.D2;

        int expected = numberOfDice;

        // Act  (Do the experiemnt)
        var diceTray = testObject.Roll(numberOfDice, mathRock);

        //Assert
        Assert.Equal(expected, diceTray.RollCount);
    }
}