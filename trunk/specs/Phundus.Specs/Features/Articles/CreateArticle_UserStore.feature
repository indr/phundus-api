Feature: Create article in user store

Background: 
	Given I am logged in as a user
	And I opened my store


Scenario: Create article in user store
	When I create an article in my store
	Then I should see ok

Scenario: Created articles are in query result
	Given I created an article in my store
	And I created an article in my store
	When I try to query all my articles
	Then I should see at least 2 articles
