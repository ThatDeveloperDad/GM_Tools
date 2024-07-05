namespace GameTools.DiceEngine;

public class DiceBag : IDiceBag
{
    public DiceTray Roll(int numberOfDice, MathRockKind mathRockKind, int resultModifier = 0)
    {
        DiceTray result = new DiceTray(resultModifier);
        
        for(int roll = 0; roll < numberOfDice; roll++)
        {
            var rollResult = new Random();

            int value = rollResult.Next(1, (int)mathRockKind +1);
            result.AddRoll(mathRockKind, value);
        }

        return result;
    }
}
