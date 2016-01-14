Feature: PlaceOrder
	
Background:
	Given an organization "Scouts" with these members
    | Alias | Role    |
	| Greg  | Manager |
	| Alice | Member  |
	And with these organization articles
	| Alias    |
	| Apple    |
	| Banana   |
	| Cucumber |
	And I am logged in as Alice

Scenario: Place order
	Given I added "Apple" to cart
	When I try to place an order for "Scouts"
	Then I should get an order id
