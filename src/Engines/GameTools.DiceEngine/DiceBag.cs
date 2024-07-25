namespace GameTools.DiceEngine;

/// <summary>
/// Provides a simple implementation of <see cref="IDiceBag"/> using the <see cref="Random"/> number generator.
/// Be aware that this implementation is not TRULY random and so is not appropriate for 
/// use cases that require cryptographically secure random characters.
/// 
/// It's just dice, dude.
/// </summary>
public class DiceBag : IDiceBag
{
    /// <inheritdoc />
    public DiceTray Roll(int numberOfDice, 
                         MathRockKind mathRockKind, 
                         int rollAdjustment = 0,
                         RollModifierKind rollModifier = RollModifierKind.None)
    {
        DiceTray tray = new DiceTray(rollAdjustment, rollModifier);
        
        if(rollModifier!=RollModifierKind.None)
        {
            numberOfDice++;
        }

        for(int roll = 0; roll < numberOfDice; roll++)
        {
            //var rollResult = new Random();

            //int value = rollResult.Next(1, (int)mathRockKind +1);
            int value = GetRandomBetween(1, (int)mathRockKind);

            tray.AddRoll(mathRockKind, value);
        }

        return tray;
    }

    public bool CoinToss()
    {
        // Get a value of 1 or 2, then subtract 1 to give us a value of 0 or 1.
        int value = Roll(1, MathRockKind.D2).Result - 1;

        //  If value = 0, we're false. If Value = 1, we're true.
        bool result = value > 0;

        return result;
    }

    public int ApplyFuzzFactor(int baseValue, int fuzzPercent)
    {
        // We'll calculate our plusMinus amount by getting whatever fuzzPercent of baseValue is.
        // ie:  If baseValue = 100, and fuzzPercent = 20, then our plusMinusAmount is 20.
        // but if baseValue = something else, then our plusMinusAmount will change proportionately.
        double fuzzAmount = (fuzzPercent * baseValue) / 100.0;
        
        int plusMinus = (int)(fuzzAmount / 2);

        int fuzzedValue = ApplyVarianceRange(baseValue, plusMinus);

        return fuzzedValue;
    }

    public int ApplyVarianceRange(int baseValue, int plusMinusAmount)
    {
        int rangeSize = plusMinusAmount * 2;
        int adjustment = GetRandomBetween(0, rangeSize) - plusMinusAmount;

        int adjustedValue = baseValue + adjustment;

        return adjustedValue;
    }

    public int GetRandomBetween(int min, int max)
    {
        var rollResult = new Random();

        int value = rollResult.Next(min, max + 1);

        return value;
    }
}
