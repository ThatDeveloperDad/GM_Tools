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
@ignore @UnhappyPath
Scenario:  RulesetAccess is not configured
	Given Ruleset Access is not properly configured
	When I call GetNpcOptions
	Then I should receive an empty dictionary
	#  Not sure I like this behavior.  If Ruleset Access is null, we have a Fatal failure mode.

		# Ignore this scenario for now while we get ONE BDD test running and passing
	# so that we can start working on some automation goodness.
@ignore @UnhappyPath
Scenario: Empty Lists of CharacterOptions in the Dictionary.
	Given Ruleset Access is configured but the Ruleset is missing required template collections.
	When I call GetNpcOptions
	Then I should receive a Dictionary with empty arrays for the missing option sets.
	# Again, This feels like a Failure mode that should keep the app from starting.
