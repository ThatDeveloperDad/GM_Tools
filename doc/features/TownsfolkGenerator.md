#Townsfolk Generator Detailed Requirements

From the main [Requirements](/doc/REQUIREMENTS.md) document, we have:

## Functional Requirements
1. Create random NPCs that the players encounter on their adventures.  
    a. Each one needs a name  
    b. What do they look like?  
    c. What is their personality like?  
    d. How tough are they?
2. We're going to need to store some lists that we can draw from to create names, appearances, personalities.
3. We'll need a way to generate each NPC's basic game stats.
4. We should be able to supply basic information about a character to be generated, when we have it.  (Their profession, their species if I already know it, etc...)
5. When we've generated an NPC, we should save that character's information to a file that we can access elsewhere.
6. If practical:  Generate the basic information for an NPC, with suggestions for all those details, and feed that to an LLM so that a more detailed and rich profile for these characters can be created.
- We are going to limit the "stored" choices that the application can use in generating an NPC for us to the material in the <a href="https://media.wizards.com/2023/downloads/dnd/SRD_CC_v5.1.pdf" target="_blank">D&D 5e SRD</a>
- We may want to implement this for OTHER game systems in the future, so we should make sure we design the core components in a way that allows us to do this.


## Technical Requirements
1. The components we create for this application should be as reuseable as is practical.
2. We do not yet want reliance on "external" resources like databases, AI Models or anything like that.  This application should run locally on the end user's own hardware.
3. As this application grows, we may need to move some capabilities into their own component areas, perhaps even run them as separate background services, if we decide other applications can use those capabilities.  
    a. Do Not build this as a microservices system now.  We simply want the internal code to be organized in a way that allows us to pull pieces out later, IF we need to.
4. For now, this will be a local application with a simple user interface.  There's no need for a fancy graphical UI, just yet.
5. The application will be built in C#, against .Net8
6. We will add automateable tests as appropriate as we implement our components.
7. If LLM usage is built, let's target a small language model that can be run on the end-user's (my) hardware, rather than paying gobs of cash to a cloud service provider for GPT4.

## NPC Generation Process
1.  Gather basic information.  
    a. Profession  (supply or roll)  
    b. Species     (supply or roll)  
    c. Background(???) (supply or roll)  
    
2.  Derive the things we can derive from Species and Profession
    a. Height  
    b. Weight  
    c. Appearance  
    d. Name

3.  Generate Game Stats  
    a. Select Rolled stats or Templated stats  
    b. Assume Level 0 Commoner template.  
    c: Make adjustments to basic abilities from species.

4.  Generate Personality from Profession, Species and Stats.

> We've identified a Data Type!  GameCharacter

> We've also identified a Resource, and an Engine.  (Ruleset, DiceBag respectively.)

> We are NOT going to account for AI augmentation during our initial design.

### What is a RuleSet?
A ruleset is a collection of named lists or other objects that define the choices and affects of those choices that exist within a particular game's rules.

An RPG Ruleset is a more specific version of that concept.  It may include lists of character species, professions, and backgrounds.  Each item in those lists may modify a character that has those attribute values.

These Rulesets will primarily manifest as Data, surrounded by Constraints.  Sounds like a Resource to me.

### What is the DiceBag and why are you calling it an Engine?
An Engine is a component of our software that DOES something.  (Logic, math, etc...)  
The DiceBag is what we're calling our Random Generator Engine for now.