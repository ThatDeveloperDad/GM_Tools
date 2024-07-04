namespace GameTools.DiceEngine;

public class RolledMathRock
{

    public RolledMathRock(MathRockKind kind, int value)
    {
        Kind = kind;
        Value = value;
    }

    public MathRockKind Kind { get; private set; }

    public int NumSides => ((int)Kind);

    public int Value {get; private set;}
}
