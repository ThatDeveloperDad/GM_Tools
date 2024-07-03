# Architecture

First, we'll define our static design.  This will include diagrams and descriptions of the different components that will form our GM_Tools solution.

Second, we'll begin the detailed design of each component.

Third, We'll cross reference our components with each other to identify their dependencies.  This exercise will help us determine which components need to be built at a higher priority than others.

---

## Static Design
![Static Architecture diagram showing the components of our Townsfolk Generator](/doc/images/TownsfolkGen_StaticArch.png)

### Component Manifest
**Consumption Layer**  

_**Client**_:  THis, for now, is just a console application.

**Logic Layer**

_**TownsfolkManager**_:  composes the capabilities and functions we'll add to the Engines and ResourceAccess components to perform the different use cases we'll have when generating Townsfolk.

_**DiceEngine**_:  Handles random number generation using a "Dice Roll" metaphor.  It will accept arguments that dewcribe how many and what kind of dice, along with optional modifiers for the result, and HOW those dice should be rolled.

_**NPCAccess**_:  Allows us to save and retrieve NPCs as we generate them.

_**RulesetAccess**_:  Gives us access to various lists, and game rules that we'll need when generating an NPC for our game.

**Data Layer**  

_**RuleSet Data**_:  For now, we're going to do this as a bunch of hard-coded C# classes.  I don't want to use a SQL Server or anything else like that, because that' too complicated for what I'm trying to demonstrate.

_**App Data**_:  For this first version, we're just going to store the characters we generate in a folder as one file / character.

---

## Detailed Designs
- /ConsoleClient:  Simple console app we'll use to exercise the components, and build a VERY simple, quick & dirty UI for this.  (It's a temporary thing.)
- /Managers/GameTools.TownsfolkManager:  Defines and exposes the Use Cases that our application will facilitate.
- /Engines/GameTools.DiceEngine:  Rolls dice, allows for result modifications, and different ways to make rolls.
- /ResourceAccess/GameTools.NPCAccess:  This allows us to save and load NPCs that we've generated.
- /ResourceAccess/GameTools.RulesetAccess: Defines common concepts for different rules in Tabletop RolePlaying Games.


## Dependency Assessment

| Component | Dependencies |
| --------- | ------------ |
| Clients | TownsfolkManager* |
| TownsfolkManager | DiceEngine, RulesetAccess, NPCAccess |
| DiceEngine | |
| RulesetAccess | collection{of RulesetDefinitions} |
| NPCAccess | Configured folder on the file system |  

*The TownsfolkManager will, at first, be referenced directly from our client.  IF we add a web interface, and decide to move the Logic into its own discrete service, we'll need to put a web API in front of it.  For now?  We'll keep things simple.