Feature: Create article in organizations store

Background:
	Given an organization with these members
	| Alias | Role    |
	| Greg  | Manager |
	And I am logged in as Greg


Scenario: Create article in organizations store
	When I create an article in the default store
	Then I should see ok

Scenario: Created articles are in query result
	Given I created an article in the default store
	And I created an article in the default store
	When I try to query all the organizations articles
	Then I should see 2 articles 
