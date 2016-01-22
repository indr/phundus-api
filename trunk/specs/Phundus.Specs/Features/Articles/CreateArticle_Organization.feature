Feature: Create article in organizations store

Background:
	Given an organization with these members
	| Alias | Role    |
	| Greg  | Manager |
	And I am logged in as Greg


Scenario: Create article in organizations store
	When I create an article in the default store with these values
	| Alias | Gross stock | Member price | Public price |
	| Apple | 3           | 1.50         | 1.60         |
	Then I should see ok
	And the article "Apple" should equal
	| Gross stock | Member price | Public price |
	| 3           | 1.50         | 1.60         |

Scenario: Created articles are in query result
	Given I created an article in the default store
	And I created an article in the default store
	When I try to query all the organizations articles
	Then I should see 2 articles 
