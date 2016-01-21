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
	And a confirmed user "John" with email address "john@test.phundus.ch"

Scenario: Place order returns order id
	Given I am logged in as Alice	
	And I added "Apple" to cart
	When I try to place an order for "Scouts"
	Then I should get an order id

Scenario: Place order sends email order received
	Given I am logged in as Alice	
	And I added "Banana" to cart
	When I try to place an order for "Scouts"
	Then "greg@test.phundus.ch" should receive email "[phundus] Neue Bestellung"

Scenario: Place order removes items from cart
	Given I am logged in as Alice	
	And I added "Cucumber" to cart
	When I try to place an order for "Scouts"
	Then my cart should be empty

Scenario: Place order as non member when public rental is activated, succeeds
	Given I am logged in as Greg
	And I set organization setting public rental on
	And I am logged in as John
	And I added "Apple" to cart
	When I try to place an order for "Scouts"
	Then I should get an order id

Scenario: Place order as non member when public rental is deactivated, fails
	Given I am logged in as Greg
	And I set organization setting public rental on
	And I am logged in as John
	And I added "Apple" to cart
	And I am logged in as Greg
	And I set organization setting public rental off
	And I am logged in as John
	When I try to place an order for "Scouts"
	Then I should see forbidden
	