Feature: CartItems
	
Background:
	Given I am logged in as "Lessor"
	And I opened my store
	And I created an article in my store

Scenario: Add user article to cart
	Given I am logged in as a user
	When I add article to cart
	Then I should see no content
