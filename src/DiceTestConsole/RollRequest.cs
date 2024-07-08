using System;
using GameTools.DiceEngine;

namespace DiceTestConsole;

public class RollRequest
{

    public RollRequest(int numberOfDice,
                       MathRockKind kindOfDice,
                       int adjustment = 0,
                       RollModifier modifier = RollModifier.None)
    {
        NumberOfDice = numberOfDice;
        KindOfDice = kindOfDice;
        Adjustment = adjustment;
        Modifier = modifier;
    }

    public int NumberOfDice{ get; set; }

    public MathRockKind KindOfDice { get; set; }

    public int Adjustment {get; set; }

    public RollModifier Modifier {get;set;}
}
