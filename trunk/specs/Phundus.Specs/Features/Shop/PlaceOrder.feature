Feature: PlaceOrder
	
Background:
	Given an organization "Scouts" with these members
    | Alias | Role    | Email address         |
    | Greg  | Manager | greg@test.phundus.ch  |
    | Alice | Member  | alice@test.phundus.ch |
	And with these organization articles
	| Alias    |
	| Apple    |
	| Banana   |
	| Cucumber |
	And I am logged in as Alice	

Scenario: Place order returns order id
	Given I added "Apple" to cart
	When I try to place an order for "Scouts"
	Then I should get an order id

Scenario: Place order sends email order received
	Given I added "Banana" to cart
	When I try to place an order for "Scouts"
	Then "greg@test.phundus.ch" should receive email "[phundus] Neue Bestellung"

Scenario: Place order removes items from cart
	Given I added "Cucumber" to cart
	When I try to place an order for "Scouts"
	Then my cart should be empty