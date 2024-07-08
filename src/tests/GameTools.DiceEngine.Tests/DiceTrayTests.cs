using System;

namespace GameTools.DiceEngine.Tests;

public class DiceTrayTests
{
    [Fact]
    public void DiceTrayTests_ConstructorWorks()
    {
        var testObject = new DiceTray();
        int rollCountAtNew = 0;
        int resultAtNew = 0;

        Assert.NotNull(testObject);
        Assert.Equal(rollCountAtNew, testObject.RollCount);
        Assert.Equal(resultAtNew, testObject.Result);
    }

    [Fact]
    public void DiceTrayTests_CanAddMathRock()
    {
        var testObject = new DiceTray();

        testObject.AddRoll(MathRockKind.D6, 4);

        int expectedRockCount = 1;

        Assert.Equal(expectedRockCount, testObject.AllRolls.Length);
    }

    [Fact]
    public void DiceTrayTest_AddTwoSixes_ExpectResult12()
    {
        var testObject = new DiceTray();

        testObject.AddRoll(MathRockKind.D6, 6);
        testObject.AddRoll(MathRockKind.D6, 6);

        var expectedResult = 12;

        Assert.Equal(expectedResult, testObject.Result);
    }
}
