Feature: CheckAvailability

Background: 
	Given I am logged in as a user
	And I opened my store
	And I created these articles in my store
	| Name     | Stock |
	| Football | 2     |

Scenario: Check availability when available
	When I try to check availability with a quantity of 1
	Then I should see available

Scenario: Check availability when not available
	When I try to check availibility with a quantity of 3
	Then I should see not available