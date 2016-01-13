Feature: EmptyCart
	
Scenario: Can access my empty cart after sign up and log in
	Given I am logged in as a user
	When I try to view my cart
	Then my cart should be empty