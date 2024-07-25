using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GameTools.DiceEngine.Tests;

public class DeckSimulatorTests
{


    [Fact]
    public void DeckSimulatorTest_FromIntegerArray_ReturnsOne()
    {
        // Arrange
        ICardDeck testObject = new DeckSimulator();
        var deck = Enumerable.Range(1, 20).ToArray();

        // Act
        var result = testObject.PickOne(deck);

        // Assert
        Assert.Contains(result, deck);
    }

    [Fact]
    public void DeckSimulatorTest_FromIntegerArray_ShuffleIsRandom()
    {
        // Arrange
        ICardDeck testObject = new DeckSimulator();
        var deck = Enumerable.Range(1, 20).ToArray();

        // Act
        // How do we ensure that the Shuffle is different almost every time?
        // Let's run this a bunch of times, collect the results and keep a count of
        // how many times we get the same result.
        var resultAccumulator = new Dictionary<int, int>();
        for(int iteration = 0; iteration < 100; iteration++)
        {
            var result = testObject.PickOne(deck);

            if(resultAccumulator.ContainsKey(result))
            {
                int currentResultCount = resultAccumulator[result];
                int newResultCount = currentResultCount + 1;
                resultAccumulator[result] = newResultCount;
            }
            else
            {
                resultAccumulator[result] = 1;
            }
        }

        int distinctResults = resultAccumulator.Count;

        // Assert
        Assert.True(distinctResults>1);
    }
}
