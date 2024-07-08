using System.Dynamic;

namespace GameTools.DiceEngine;

/// <summary>
/// DiceTray carries the result of the Roll operation back to whichever component uses it.
/// </summary>
public class DiceTray
{
    private List<RolledMathRock> _rocks;

    public DiceTray():this(rollAdjustment:0, RollModifierKind.None){}

    public DiceTray(int rollAdjustment, RollModifierKind rollModifer)
    {
        _rocks = new List<RolledMathRock>();
        RollAdjustment = rollAdjustment;
        RollModifier = rollModifer;
    }

    public int RollAdjustment {get; private set;}

    public RollModifierKind RollModifier {get; private set;}

    public RolledMathRock[] AllRolls
    {
        get
        {
            HandleRollModifier();
            return _rocks.ToArray();
        }
    }

    public RolledMathRock[] IncludedRolls
    {
        get
        {
            HandleRollModifier();
            return _rocks.Where(r=> r.IsDiscarded == false).ToArray();
        }
    }

    public int RollCount => _rocks.Count;

    public int UnadjustedResult 
    {
        get
        {
            HandleRollModifier();
            return _rocks
                .Where(r=> r.IsDiscarded == false)
                .Sum(r=> r.Value);
        }
    }

    public int Result 
    {
        get
        {
            int adjustedResult = UnadjustedResult + RollAdjustment;
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
        _rollIsModified = false;
    }

    private bool _rollIsModified;
    private void HandleRollModifier()
    {
        if(_rollIsModified)
            return;

        _rocks.ForEach(r=> r.ResetDiscardState());

        if(RollModifier != RollModifierKind.None)
        {
            _rocks = _rocks.OrderBy(r=> r.Value).ToList();
        }

        if(RollModifier == RollModifierKind.Advantage)
        {
            // Set the lowest value to Discarded.
            _rocks.First().Discard();
            _rollIsModified = true;
        }
        else if(RollModifier == RollModifierKind.Disadvantage)
        {
            // Set the Highest value to Discarded.
            _rocks.Last().Discard();
            _rollIsModified = true;
        }
    }
}
