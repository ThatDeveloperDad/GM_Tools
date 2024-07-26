Feature: GetNPCOptions

Loads and returns the selectable character options from the 
RulesetAccess component.

@HappyPath
Scenario: Happy Path Scenario: Retrieving NPC Options Successfully
	Given RulesetAccess is properly configured
	When I call GetNpcOptions
	Then I should receive a containing valid options for species, background, and vocation

	# Ignore this scenario for now while we get ONE BDD test running and passing
	# so that we can start working on some automation goodness.
@UnhappyPath
Scenario:  RulesetAccess is not configured
	Given Ruleset Access is not properly configured
	Then GetNpcOptions throws An InvalidOperationException.
	

@UnhappyPath
Scenario: Empty Lists of CharacterOptions in the Dictionary.
	Given Ruleset Access is configured but the Ruleset is missing required template collections.
	When I call GetNpcOptions
	Then I should receive a Dictionary with empty arrays for the missing option sets.
	# Again, This feels like a Failure mode that should keep the app from starting.
