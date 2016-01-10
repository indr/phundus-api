Feature: Establish organization
	
Background: 
	Given logged in as root	

Scenario: can query for
	When establish organization
	Then query organizations should contain it

Scenario: can get details
	When establish organization
	Then get organization details