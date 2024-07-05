namespace GameTools.DiceEngine;

/// <summary>
/// DiceTray carries the result of the Roll operation back to whichever component uses it.
/// </summary>
public class DiceTray
{
    private List<RolledMathRock> _rocks;

    public DiceTray():this(resultModifier:0){}

    public DiceTray(int resultModifier)
    {
        _rocks = new List<RolledMathRock>();
        ResultModifier = resultModifier;
    }

    public int ResultModifier {get; private set;}

    public int[] Rolls => _rocks.Select(r=> r.Value).ToArray();

    public int RollCount => _rocks.Count;

    public int UnadjustedResult => _rocks.Sum(r=> r.Value);

    public int Result 
    {
        get
        {
            int adjustedResult = UnadjustedResult + ResultModifier;
            // We never want a negative result from our dice rolls.
            // If < 0, set it to 0.
            if(adjustedResult<0)
            {
                adjustedResult = 0;
            }

            return adjustedResult;
        }
    }

    public void AddRoll(MathRockKind kind, int result)
    {
        var roll = new RolledMathRock(kind, result);
        _rocks.Add(roll);
    }

}
