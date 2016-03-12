Feature: CreateEmptyOrder

Background:
	Given an organization "Scouts" with these members
	| Alias | Role    |
	| Greg  | Manager |
	| Alice | Member  |
	And I am logged in as "Greg"

Scenario: Order query shows newly created empty return
	When I create a new empty order for Alice
	Then I should see ok
	And I should find the order in the order query for organization "Scouts"

