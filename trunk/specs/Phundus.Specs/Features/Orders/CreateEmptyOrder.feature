Feature: CreateEmptyOrder

Background:
	Given an organization "Scouts" with these members
	| Alias | Role    |
	| Greg  | Manager |
	| Alice | Member  |
	And I am logged in as "Greg"

Scenario: Create empty orders returns ok	
	When I try to create a new empty order for Alice
	Then I should see ok

Scenario: Order query shows order
	Given I created a new empty order for Alice
	When I try to query all orders of organization "Scouts"
	Then I should find the order in the results

