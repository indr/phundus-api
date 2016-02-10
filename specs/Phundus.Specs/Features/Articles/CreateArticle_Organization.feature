Feature: Create article in organizations store

Background:
	Given an organization with these members
	| Alias | Role    |
	| Greg  | Manager |
	And I am logged in as Greg


Scenario: Create article in organizations store
	When I create an article in the default store with these values
	| Alias | Name   | Gross stock | Member price | Public price |
	| Apple | Boskop | 3           | 1.50         | 1.60         |
	Then I should see ok
	And the article "Apple" should equal
	| Name   | Gross stock | Member price | Public price |
	| Boskop | 3           | 1.50         | 1.60         |

Scenario: Created articles are in query result
	Given I created an article in the default store
	And I created an article in the default store
	When I try to query all the organizations articles
	Then I should see at least 2 articles 

Scenario: Update article description
	Given I created an article in the default store
	When I try to update the article description:
		"""
		This article has a
		multiline
		description.
		"""
	Then the article description is:
		"""
		This article has a
		multiline
		description.
		"""

Scenario: Update article specification
	Given I created an article in the default store
	When I try to update the article specification:
		"""
		This article has a
		multiline
		specification.
		"""
	Then the article specification is:
		"""
		This article has a
		multiline
		specification.
		"""

Scenario: Update article details
	Given I created an article in the default store
	When I try to update the article details
	| Name   | Brand  | Color | Gross stock |
	| Volley | Wilson | White | 10          |
	Then the article should equal
	| Name   | Brand  | Color | Gross stock |
	| Volley | Wilson | White | 10          |