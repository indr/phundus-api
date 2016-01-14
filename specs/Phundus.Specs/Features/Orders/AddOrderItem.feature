Feature: AddOrderItem

Background: 
	Given an organization "Scouts" with these members
    | Alias | Role    |
	| Greg  | Manager |
	| Alice | Member  |
	And with these organization articles
	| Alias    |
	| Football |
	And I am logged in as Greg
	And I created a new empty order for Alice


Scenario: Add article returns ok
	When I try to add the article Football to the order
	Then I should see created
