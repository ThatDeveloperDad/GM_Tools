# Notes
These are just notes we made while talking through the Requirements fo rthe Townsfolk Generator.

| **Concept** | **Noun/Verb** | **Shared?** | **Changes?** |
|---------|-----------|---------|----------|
| NPC | Noun | Y |  |
| NpcSeed* | Noun |  |  |
| SpeciesList | Noun | Y |  |
| AppearancesList | Noun | Y |  |
| NameLists | Noun | Y |  |
| BackgroundList | Noun | Y |  |
| ProfessionList | Noun | Y |  |
| ~~RollDice~~ | ~~Verb~~ |  |  |
| DiceBag | Service | Y |  |
| SaveNPC | Verb |  | Y |

*Items marked with a * aren't VOLATILE, but they are VARIABLE.
NPCSeed is a set of basic attributes that may or may not be used to pre-define certain aspects of a Townsperson.

** RollDice is both shared and volatile in its behavior from one execution to the next.
That means we need to look at it more closely, and break that concept down further untit we have smaller pieces that are volatile along exactly one of those axes.

What changes when we roll dice in our game?
- The number of dice might be different for each instance.
- The Kind of dice may also vary for each instance.
- We may apply modifiers to the result of those dice in different circumstances.
- We may ALSO have situations where we roll more than the specified number of dice, and ignore the lowest or highest when calculating the result.  (Advantage/Disadvantage)

What if we treat RollDice as a single thing that we can do?
What if we made those things that can vary into parameters in our function?
That means, we:

// HowMany is the number of dice to roll.  
// WhatKind is the type of dice we'll use.  
// ResultModifiers will add or subtract a provided value from the result once its been calculated.  
// RollModifier is a special condition that will change the number of dice that are rolled, and how they are totalled.  
RollDice(HowMany?, WhatKind?, ResultModifiers?, RollModifier?)

Let's wrap all of these concepts up into a single "section" of our code when we get to it.  We'll call that section a DiceEngine.  (That's the label we'll use for the box we'll put all the DiceStuff into.)  We'll call the thing that owns the FUNCTION of rolling the dice a DiceBag.