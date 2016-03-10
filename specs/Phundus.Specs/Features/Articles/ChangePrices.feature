Feature: Change article prices


Scenario: Change organization article prices
	Given an organization with these members
	| Alias | Role    |
	| Greg  | Manager |
	And I am logged in as Greg
	And I created an article in the default store
	When I try to change the price to 10.00 and 8.00
	Then the article should equal
	| Public price | Member price |
	| 10.00        | 8.00         |

Scenario: Change user article prices
	Given I am logged in as a user
	And I opened my store	
	And I created an article in my store
	When I try to change the price to 11.00 and 9.00
	Then the article should equal
	| Public price | Member price |
	| 11.00        |              |