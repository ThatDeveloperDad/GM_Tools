Feature: GenerateTownsperson

This feature accepts zero or any available user-selected
attributes for the Townsperson to Generate.  Those
attributes that are not specified should be selected randomly
from the value lists provided by the RuleSet in use.

@happy-path
Scenario: UserProvidesNoSelectedAttributes
	Given The TownsfolkManager is Ready to Generate NPCs
	When The user Requests an NPC without specifying options
	Then The GenerateTownsperson Method will return a non-null value
	And the return value will be a Townsperson instance
	And the return value will have a Species
	And the return value will have a Vocation
	And the return value will have a Background

@happy-path
Scenario Outline: RandomNpcsWithUserSelections
	Given The TownsfolkManager is Ready to Generate NPCs
	When the user supplies attributes with the NPC request
	And the requested Species is <species>
	And the requested Background is <background>
	And the requested Vocation is <vocation>
	And the options are sent to the TownsfolkManager
	Then The GenerateTownsperson Method will return a non-null value
	And the return value will be a Townsperson instance
	And the Townsperson will have their Species set to <species>
	And the Townsperson will have their Background set to <background>
	And the Townsperson will have their Vocation set to <vocation>

	Examples: 
	| species    | background | vocation |
	| "<null>"   | "<null>"   | "<null>"  |
	| "Human"   | "<null>"   | "<null>"  |
	| "<null>"   | "Guild Artisan"   | "<null>"  |
	| "<null>"   | "<null>"   | "Shop Keeper"  |
	| "Elf"   | "Noble"   | "<null>"  |
	| "Dwarf"   | "<null>"   | "Leatherworker"  |
	| "<null>"   | "Soldier"   | "Jeweler"  |
	| "Halfling"   | "Urchin"   | "Thief"  |
	

