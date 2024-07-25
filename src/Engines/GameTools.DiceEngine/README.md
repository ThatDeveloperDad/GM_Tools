# GameTools.DiceEngine  (Component Library)

## Root Namespace (GameTools.DiceEngine)
Holds the default implementation of the DiceBag component.

---
### Component Implementations
#### DiceBag (class)
_Implements:_ [GameTools.DiceEngine.Contracts.IDiceBag](#idicebag)

**Public Methods**
| Name | Returns |  Description |
| ---- | --- |  --- |
| Roll | [DiceTray](#dicetray-class) | Parameters:<br>**Required:** int numberOfDice<br /> **Required:** [MathRockKind](#mathrockkind-enum) diceKind<br />_Optional:_ int rollAdjustment<br />_Optional:_ [RollModifierKind](#rollmodifierkind-enum) rollModifier <hr> **Description:** Simulates rolling a number of dice of the specified kind.<br />  Optionally, an adjustment can be provided in the rollAdjustment parameter (default 0) that is added to the result in the Dice Tray.<br />  Optionally, the behavior of the roll can be modified by supplying a value for rollModifier (default None) that will roll the dice with Advantage or Disadvantage.<br />The simulated Dice are returned in the DiceTray. |

*Usage*
```csharp
    IDiceBag diceBag = new DiceBag();
    
    // Roll 3d6 without any adjustments or modifications.
    var simpleRoll = diceBag.Roll(numberOfDice: 3,
                                  diceKind: MathRockKind.D6);
    int strengthScore = simpleRoll.Result;
    // Result:  Something between 3 and 18.

    // Roll 1d10 + 2
    var damageRoll = diceBag.Roll(numberOfDice: 1,
                                  diceKind: MathRockKind.D10,
                                  rollAdjustment: 2);
    int damage = damageRoll.Result;
    // Result: Something between 1 and 10 (+2), so 3 and 12.

    // Make an attack roll with Advantage
    var attackRoll = diceBag.Roll(numberofDice: 1,
                                  diceKind: MathRockKind.D20,
                                  rollModifier: RollModifierKind.Advantage);
    var attack = attackRoll.Result;
    // Result:  Rolls 2 D20, picks the higher roll.

    // Make an attack roll with +2 to hit, but at Disadvantage
    var attackRoll = diceBag.Roll(numberofDice: 1,
                                  diceKind: MathRockKind.D20,
                                  rollAdjustment: 2,
                                  rollModifier: RollModifierKind.Disadvantage);
    var attack = attackRoll.Result;
    // Result:  Rolls 2 D20, picks the lower roll and adds 2.
```
---

## Contracts Namespace (GameTools.DiceEngine.Contracts)
Contains the public contracts that describe the types and services provided by the GameTools DiceEngine.

---
### Service Contracts
#### IDiceBag
Defines the behavior of a simulated bag of dice commonly used in tabletop role playing games.  

**Methods**
| Name | Returns |  Description |
| ---- | --- |  --- |
| Roll | [DiceTray](#dicetray-class) | Parameters:<br>**Required:** int numberOfDice<br /> **Required:** [MathRockKind](#mathrockkind-enum) diceKind<br />_Optional:_ int rollAdjustment<br />_Optional:_ [RollModifierKind](#rollmodifierkind-enum) rollModifier <hr> **Description:**<br /> Simulates rolling a number of dice of the specified kind.<br />  Optionally, an adjustment can be provided in the rollAdjustment parameter (default 0) that is added to the result in the Dice Tray.<br />  Optionally, the behavior of the roll can be modified by supplying a value for rollModifier (default None) that will roll the dice with Advantage or Disadvantage.<br />The simulated Dice are returned in the DiceTray. |

---
### Public Types
Classes and enums that are exposed by the DiceEngine components.  

#### DiceTray (class)
Is the return type from the Roll() method on IDiceEngine and its implementations.

**Constructors**
| Signature | Description |
| --- | --- |
| DiceTray() | Creates a new DiceTray instance with the default Adjustment (0), and default RollModifierKind (None). |
| DiceTray(int rollAdjustment, RollModifierKind rollModifier) | Creates a new DiceTray instance with the provided adjustment and RollModifierKind values. |

**Properties**
| Name | Type | Direction | Notes |
| --- | --- | --- | --- |
| AllRolls | RolledMathRock[] | get | Returns all Dice rolled into the tray |
| IncludedRolls | RolledMathRock[] | get | Returns only those Dice that are included in the Result calculation.<br/>(This ignores and extra Roll added by a RollModifier.) |
| Result | int | get | Returns the sum of the Included Roll values + any RollAdjustment.<br />This value will never return less than zero. |
| RollAdjustment | int | get<br />pvt set | Specifies the amount to add or subtract to the included rolls when calculating the Result. |
| RollCount | int | get | Returns the total number of Dice rolled into this tray, whether included in the Result or not. |
| RollModifier | RollModifierKind | get<br />pvt set | Identifies which (if any,) Modifier was chosen for this roll. |
| UnadjustedResult | int | get | Returns the total of all IncludedRolls in the tray, without the RollAdjustment.|

> _Note that all properties are effectively read only and may not be modified directly from outside the class._


**Public Methods**
| Name | Returns |  Description |
| ---- | --- |  --- |
| AddRoll | void | Parameters:<br />**Required:** MathRockKind kind<br /><br>**Required:** int result<br /> <hr> **Description:** Adds one roll of the spcified kind of MathRock to the Tray, with its generated value. As each roll is added to the Tray, a private flag is set to force the private ModifierHandler to run when a Result property is accessed. |

#### RolledMathRock (class)
Represents the state of one Dice Roll in the Dice Tray.  

**Constructors**
| Signature | Description |
| --- | --- |
| RolledMathRock(MathRockKind kind, int value) | Creates a new RolledMathRock instance and sets the kind of Dice with its rolled Value.  Its Discard value, by default, is false. |

**Properties**
| Name | Type | Direction | Notes |
| --- | --- | --- | --- |
| IsDiscarded | bool | get<br />pvt set | Indicates whether this instance should be ignored when calculating the Result properties in the parent DiceTray instance. |
| Kind | MathRockKind | get<br />pvt set | The kind of MathRock represented by this instance. |
| Value | int | get<br />pvt set | The value of the Roll represented by this instance. |

**Internal Methods**  
Note:  These methods are used by the DiceTray class when calculating results for Modified rolls.  They should not be used for any other purpose.
| Name | Returns |  Description |
| ---- | --- |  --- |
| SetDiscarded | void | **Description:**<br /> Sets the IsDiscarded flag for this instance to true. |
| UnsetDiscarded | void | **Description:**<br /> Sets the IsDiscarded flag for this instance to false. |

#### MathRockKind (enum)
Defines the different valid kinds of Dice we can roll.  
The Value of these enum members identifies the number of Sides for the named MathRock Kind.

**Members**
| Name | Value | 
| --- | --- |
| D2 | 2 |
| D4 | 4 |
| D6 | 6 |
| D8 | 8 |
| D10 | 10 |
| D12 | 12 |
| D20 | 20 |
| D100 | 100 |

#### RollModifierKind (enum)
Defines the ways we can modify a DiceRoll using a component from the DiceEngine library.  This enum's members do not have numeric values associated with them.

**Members**
| Name | Effect |
| --- | --- |
| None | The number of dice rolled is unmodified, and all are included in the Result calculations. |
| Advantage | One extra mathrock of the requested kind is added to the DiceTray.  The lowest value is excluded from the Result calculations. |
| Disadvantage | One extra mathrock is added to the DiceTray.  The highest value is excluded from the Result calculations. |