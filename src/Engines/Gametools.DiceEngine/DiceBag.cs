namespace GameTools.DiceEngine;

public class DiceBag : IDiceBag
{
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
