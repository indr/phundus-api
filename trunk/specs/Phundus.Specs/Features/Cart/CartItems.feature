Feature: CartItems
	
Background:
	Given I am logged in as "Lessor"
	And I opened my store
	And I created an article in my store

Scenario: Add user article to cart
	Given I am logged in as a user
	When I try to add article to cart
	Then I should see ok

Scenario: Remove cart item, succeeds
	Given I am logged in as a user
	And I added an article to cart
	When I try remove the last cart item
	Then I should see no content
