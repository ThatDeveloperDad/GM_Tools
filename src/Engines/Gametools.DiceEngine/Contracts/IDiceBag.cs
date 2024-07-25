namespace GameTools.DiceEngine;

/// <summary>
/// IDiceBag defines the behaviors and features of the Dice Rolling component in our system.
/// </summary>
public interface IDiceBag
{

    /// <summary>
    /// Generates a random number by simulating the roll of a number of dice, 
    /// each of which has a number of sides determined by the diceKind, and possibly
    /// modify the sum of those dice by the resultModifier.
    /// </summary>
    /// <param name="numberOfDice">How many dice to roll</param>
    /// <param name="diceKind">How many sides do these dice have?</param>
    /// <param name="rollAdjustment">(Optional) Do we need to change the result before returning it?</param>
    /// /// <param name="rollModifier">(Optional) Alters the number of dice to be rolled and the way the Result is calculated.</param>
    /// <returns></returns>
    DiceTray Roll(int numberOfDice, MathRockKind diceKind, int rollAdjustment = 0, RollModifierKind rollModifier = RollModifierKind.None);

    /// <summary>
    /// Chooses and random boolean value, simulating a coin toss.
    /// </summary>
    /// <returns></returns>
    bool CoinToss();

    /// <summary>
    /// Calculates a random number within fuzzPercent of the baseValue
    /// </summary>
    /// <param name="baseValue"></param>
    /// <param name="fuzzPercent"></param>
    /// <returns></returns>
    int ApplyFuzzFactor(int baseValue, int fuzzPercent);

    /// <summary>
    /// Calculates a random number that is plusMinusAmount around baseValue.
    /// </summary>
    /// <param name="baseValue"></param>
    /// <param name="plusMinusAmount"></param>
    /// <returns></returns>
    int ApplyVarianceRange(int baseValue, int plusMinusAmount);

    int GetRandomBetween(int min, int max);

}
