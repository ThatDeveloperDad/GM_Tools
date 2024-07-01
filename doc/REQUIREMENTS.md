# Requirements
This document is separated into two different sections.  We'll list FUNCTIONAL requirements first, then we'll list TECHNICAL requirements.

**Links**
- [Feature Wish List](#feature-wishlist)
- [Functional Requirements](#functional-requirements)
- [Technical Requirements](#technical-requirements)
- [Project Overview](../README.md)

## Feature WishList
| Feature | Short Description | Priority |
|---------|-------------------|----------|
| [Townsperson Generator](#townsfolk-generator) | Randomly generate some details about NPCs that the party encounters | MVP |
| Initiative tracking | It'd be nice to have an app where I can store the encounter characters and have their initiative order tracked for me | later, if ever |

## Functional Requirements
_**What does this thing need to do?**_
### Townsfolk Generator
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


## Technical Requirements
_**How does it do those things, and what other nerdy stuff matters for this?**_
1. The components we create for this application should be as reuseable as is practical.
2. We do not yet want reliance on "external" resources like databases, AI Models or anything like that.  This application should run locally on the end user's own hardware.
3. As this application grows, we may need to move some capabilities into their own component areas, perhaps even run them as separate background services, if we decide other applications can use those capabilities.  
    a. Do Not build this as a microservices system now.  We simply want the internal code to be organized in a way that allows us to pull pieces out later, IF we need to.
4. For now, this will be a local application with a simple user interface.  There's no need for a fancy graphical UI, just yet.
5. The application will be built in C#, against .Net8
6. We will add automateable tests as appropriate as we implement our components.
7. If LLM usage is built, let's target a small language model that can be run on the end-user's (my) hardware, rather than paying gobs of cash to a cloud service provider for GPT4.