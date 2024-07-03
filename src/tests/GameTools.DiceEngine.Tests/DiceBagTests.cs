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
        var result = testObject.Roll(numberOfDice, mathRock);
        
        // Assert (Check our results!)
        Assert.True(result>0);
    }

    // The result of the unmodified Roll is at least the number of dice we rolled.

    // The result of the unmodified Roll is no more than NumberOfDice x DiceKind.

    // We rolled the correct number of dice.
}