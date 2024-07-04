namespace GameTools.DiceEngine;

/// <summary>
/// DiceTray carries the result of the Roll operation back to whichever component uses it.
/// </summary>
public class DiceTray
{
    private List<RolledMathRock> _rocks;

    public DiceTray()
    {
        _rocks = new List<RolledMathRock>();
    }

    public int[] Rolls => _rocks.Select(r=> r.Value).ToArray();

    public int RollCount => _rocks.Count;

    public int Result => _rocks.Sum(r=> r.Value);

    public void AddRoll(MathRockKind kind, int result)
    {
        var roll = new RolledMathRock(kind, result);
        _rocks.Add(roll);
    }

}
