using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.CompilerServices;
using GameTools.DiceEngine;
// See https://aka.ms/new-console-template for more information

namespace DiceTestConsole;

public class Program
{
    static void Main(string[] args)
    {
        foreach(var arg in args)
        {
            Console.Write(arg + "      ");
        }
        Console.WriteLine();

        List<RollRequest> paramCombos = new List<RollRequest>();
        int numDice = 3;
        MathRockKind diceKind = MathRockKind.D6;
        int adjustment = 0;
        RollModifierKind modifier = RollModifierKind.None;

        int batchSize = 6;

        var commandArgs = Environment.GetCommandLineArgs().ToList();

        if(commandArgs.Contains("--help", StringComparer.OrdinalIgnoreCase))
        {
            Console.WriteLine("Valid args to pass are:");
            Console.WriteLine();
            Console.WriteLine("-n=[int]     : How many dice to roll?");
            Console.WriteLine("-d=[D#]      : What kind of MathRock? (All the standards are here)");
            Console.WriteLine("-a=[int]     : Add or subtract this from the result.");
            Console.WriteLine("-b=[int]     : Batch size (default 6).");
            Console.WriteLine("-withAdvantage   : Rolls with advantage.");
            Console.WriteLine("-withDisadvantage : Rolls with disadvantage.");
            Console.WriteLine();
            Console.WriteLine("Example:  dotnet DiceTestConsole.dll -n 3 -d D6 -withAdvantage");
            Console.WriteLine("Will roll 3d6 plus an extra, but ignore the lowest roll on the result.");
            return;
        }

        var parsedArgs = ParseCommandArgs(commandArgs);
        if(parsedArgs.TryGetValue("NumberOfDice", out object? numberOfDice)
          && int.TryParse(numberOfDice.ToString(), out int parsedNumDice))
        {
            numDice = parsedNumDice;
        }

        if(parsedArgs.TryGetValue("KindOfDice", out object? kindOfDice)
          && Enum.TryParse(kindOfDice.ToString(), out MathRockKind parsedKindOfDice))
        {
            diceKind = parsedKindOfDice;
        }

        if(parsedArgs.TryGetValue("Adjustment", out object? adjustmentValue)
            && int.TryParse(adjustmentValue.ToString(), out int parsedAdjustment))
        {
            adjustment = parsedAdjustment;
        }

        if(parsedArgs.TryGetValue("RollModifier", out object? modifierValue) &&
            Enum.TryParse(modifierValue.ToString(), out RollModifierKind parsedModifier))
        {
            modifier = parsedModifier;
        }

        if(parsedArgs.TryGetValue("BatchSize", out object? batchSizeValue) &&
            int.TryParse(batchSizeValue.ToString(), out int parsedBatchSize))
        {
            batchSize = parsedBatchSize;
        }
        
        paramCombos.Add(new RollRequest(numDice, diceKind, adjustment, modifier));

        TestDice(paramCombos, batchSize);
    }

    static void TestDice(List<RollRequest> paramCombos, int iterationsPerCombo)
    {
        IDiceBag diceBag = new DiceBag();
        List<DiceTray> sampleSet = new List<DiceTray>();
        
        foreach(RollRequest rr in paramCombos)
        {
            Stopwatch stopwatch= new Stopwatch();
            stopwatch.Start();
            for(int iteration = 0; iteration < iterationsPerCombo; iteration++)
            {
                DiceTray sample = diceBag.Roll(rr.NumberOfDice, rr.KindOfDice, rr.Adjustment, rr.Modifier);
                sampleSet.Add(sample);
                if(iterationsPerCombo<=10){
                    PrintSample(sample);
                }
                
            }
            stopwatch.Stop();
            long elapsedMs = stopwatch.ElapsedMilliseconds;
            PrintSummary(sampleSet, iterationsPerCombo, elapsedMs);
        }
    }

    static void PrintSample(DiceTray sample)
    {
        string mathRockName = sample.AllRolls[0].Kind.ToString();
        string modifierText = "";
        if(sample.RollModifier != RollModifierKind.None)
        {
            modifierText = $" with {sample.RollModifier.ToString()}";
        }
        string sampleOutput = $"Rolling: {sample.IncludedRolls.Count()}{mathRockName}{modifierText}";
        string allDice = string.Join(", ", sample.AllRolls.Select(r=> r.IsDiscarded? $"Ignored:{r.Value}" : r.Value.ToString() + ""));
        
        string sampleResult = $"All Dice: {allDice}  FinalResult:  {sample.Result} ";

        Console.WriteLine(sampleOutput);
        Console.WriteLine(sampleResult);

    }

    static void PrintSummary(List<DiceTray> sampleSet, int sampleSize, long runTimeMs)
    {
        Func<int, int, decimal> calcPercent 
            = (count, sampleSize) => 
            {
                decimal pct = ((decimal)count / (decimal)sampleSize) * (decimal)100;
                return pct;
            };


        var distinctResults = sampleSet.Select(s=> s.Result).Distinct().ToArray();
        var aggregates = distinctResults
                        .Select(r=> new 
                            { 
                                Result = r,
                                Count = sampleSet.Count(ss=> ss.Result == r),
                                Percent = calcPercent(sampleSet.Count(ss=> ss.Result == r), sampleSize)
                            })
                        .OrderBy(a=> a.Result);
        Console.WriteLine($"Ran {sampleSize} tests in {runTimeMs} milliseconds.");
        Console.WriteLine($"Result      Count       PercentOfSample");
        foreach(var stat in aggregates)
        {
            Console.WriteLine($"{stat.Result}       {stat.Count}        {stat.Percent}");
        }
    }

    static Dictionary<string, object> ParseCommandArgs(List<string> commandArgs)
    {
        Dictionary<string, object> parsedArgs = new Dictionary<string, object>();
        foreach(string arg in commandArgs)
        {
            if(arg == "--help")
            {
                // go to the next one.
                continue;
            }

            if(arg == "-withAdvantage")
            {
                parsedArgs.Add("RollModifier", RollModifierKind.Advantage);
            }
            else if(arg == "-withDisadvantage")
            {
                parsedArgs.Add("RollModifier", RollModifierKind.Disadvantage);
            }
            else if(arg.Contains("="))
            {
                // it's a switch/value pair.
                var parts = arg.Trim().Split("=");
                if(parts.Length == 1)
                {
                    // it's invalid.
                    continue;
                }
                switch(parts[0])
                {
                    case "-n":
                        parsedArgs.Add("NumberOfDice", int.Parse(parts[1]));
                        break;
                    case "-d":
                        parsedArgs.Add("KindOfDice", Enum.Parse<MathRockKind>(parts[1].ToUpper()));
                        break;
                    case "-a":
                        parsedArgs.Add("Adjustment", int.Parse(parts[1]));
                        break;
                    case "-b":
                        parsedArgs.Add("BatchSize", int.Parse(parts[1]));
                        break;
                    default:
                        break;
                }
            }
        }

         return parsedArgs;   
    }
}


