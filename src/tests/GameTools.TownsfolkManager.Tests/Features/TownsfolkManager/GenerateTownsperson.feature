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
Scenario: UserProvidesSomeSelectedAttributes
	Given The TownsfolkManager is Ready to Generate NPCs
	When the user Requests an Npc with Elf as the species
	Then The GenerateTownsperson Method will return a non-null value
	And the return value will be a Townsperson instance
	And the return value Species will be Elf
	And the return value will have a Vocation
	And the return value will have a Background