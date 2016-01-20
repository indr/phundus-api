Feature: Organization settings
	
Background:
	Given an organization "Scouts" with these members
	| Alias | Role    |
	| Greg  | Manager |
	And I am logged in as Greg

Scenario: Set public rental on
	When I try to set organization setting public rental on
	Then should the organization settings equal
	| Public rental |
	| True          |

Scenario: Set public rental off
	Given I set organization setting public rental on
	When I try to set organization setting public rental off
	Then should the organization settings equal
	| Public rental |
	| False         |
