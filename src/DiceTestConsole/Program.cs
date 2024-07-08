using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using GameTools.DiceEngine;
// See https://aka.ms/new-console-template for more information

namespace DiceTestConsole;

public class Program
{
    static void Main()
    {
        // Big giant test set.  Probably won't use it. :P
        List<RollRequest> paramCombos = new List<RollRequest>();
        paramCombos.Add(new RollRequest(3, MathRockKind.D6, 0, RollModifier.Advantage));

        TestDice(paramCombos, 6);
    }

    static void TestDice(List<RollRequest> paramCombos, int iterationsPerCombo)
    {
        IDiceBag diceBag = new DiceBag();
        List<DiceTray> sampleSet = new List<DiceTray>();

        foreach(RollRequest rr in paramCombos)
        {
            for(int iteration = 0; iteration < iterationsPerCombo; iteration++)
            {
                DiceTray sample = diceBag.Roll(rr.NumberOfDice, rr.KindOfDice, rr.Adjustment, rr.Modifier);
                sampleSet.Add(sample);
                PrintSample(sample);
            }

        }
    }

    static void PrintSample(DiceTray sample)
    {
        string mathRockName = sample.AllRolls[0].Kind.ToString();
        string modifierText = "";
        if(sample.RollModifier != RollModifier.None)
        {
            modifierText = $" with {sample.RollModifier.ToString()}";
        }
        string sampleOutput = $"Rolling: {sample.IncludedRolls.Count()}{mathRockName}{modifierText}";
        string allDice = string.Join(", ", sample.AllRolls.Select(r=> r.IsDiscarded? $"Ignored:{r.Value}" : r.Value.ToString() + ""));
        
        string sampleResult = $"All Dice: {allDice}  FinalResult:  {sample.Result} ";

        Console.WriteLine(sampleOutput);
        Console.WriteLine(sampleResult);

    }

    static void PrintSummary(List<DiceTray> sampleSet)
    {

    }

    

}

