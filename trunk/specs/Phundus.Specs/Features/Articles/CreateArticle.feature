Feature: CreateArticle

Background: 
	Given I am logged in as a user
	And I opened my store


Scenario: Add article to user store
	When I create an article in my store
	Then I should see ok

Scenario: Article is in query result
	Given I created an article in my store
	And I created an article in my store
	When I try to query all my articles
	Then I should see 2 articles
