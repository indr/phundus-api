Feature: Establish organization
	
Background: 
	Given logged in as root	

Scenario: can query for
	When establish organization
	Then query organizations should contain it

@ignore
Scenario: can get details
	When establish organization
	Then get organization details