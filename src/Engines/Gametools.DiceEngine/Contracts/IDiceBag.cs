namespace GameTools.DiceEngine;

/// <summary>
/// IDiceBag defines the behaviors and features of the Dice Rolling component in our system.
/// </summary>
public interface IDiceBag
{

    /// <summary>
    /// We will generate a random number by simulating the roll of a number of dice, 
    /// each of which has a number of sides determined by the diceKind, and possibly
    /// modify the sum of those dice by the resultModifier.
    /// </summary>
    /// <param name="numberOfDice">How many dice to roll</param>
    /// <param name="diceKind">How many sides do these dice have?</param>
    /// <param name="resultModifier">Do we need to change the result before returning it?</param>
    /// <returns></returns>
    int Roll(int numberOfDice, MathRockKind diceKind, int? resultModifier = null);

    //int Roll(int numberOfDice, MathRockKind diceKind, int? resultModifier = null, object? rollModifer = null);
}
