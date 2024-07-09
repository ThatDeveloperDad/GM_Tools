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
            var rollResult = new Random();

            int value = rollResult.Next(1, (int)mathRockKind +1);
            tray.AddRoll(mathRockKind, value);
        }

        return tray;
    }
}
