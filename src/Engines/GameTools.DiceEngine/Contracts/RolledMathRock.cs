namespace GameTools.DiceEngine;

public class RolledMathRock
{

    public RolledMathRock(MathRockKind kind, int value)
    {
        Kind = kind;
        Value = value;
    }

    public MathRockKind Kind { get; private set; }

    public int Value {get; private set;}

    public bool IsDiscarded {get; private set;}

    internal void SetDiscarded()
    {
        IsDiscarded = true;
    }

    internal void UnsetDiscarded()
    {
        IsDiscarded = false;
    }

}
