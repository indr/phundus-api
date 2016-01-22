Feature: Add organization items to cart
	
Background:
	Given an organization "Scouts" with these members
    | Alias | Role    | Email address         |
    | Greg  | Manager | greg@test.phundus.ch  |
    | Alice | Member  | alice@test.phundus.ch |
	And with these organization articles
	| Alias | Name  | Member price | Public price |
	| Apple | Apple | 1.00         | 1.10         |
	And a confirmed user "John" with email address "john@test.phundus.ch"

Scenario: Add article to cart as manager, succeeds
	Given I am logged in as Greg
	When I try to add article to cart
	Then my cart should have these items:
	| Text  | Item Total |
	| Apple | 1.00       |

Scenario: Add article to cart as member, succeeds
	Given I am logged in as Alice
	When I try to add article to cart
	Then my cart should have these items:
	| Text  | Item Total |
	| Apple | 1.00       |

Scenario: Add article to cart as non member, fails
	Given I am logged in as John
	When I try to add article to cart
	Then I should see forbidden
	And my cart should be empty

@ignore
Scenario: Add article to cart as non member when public rental is activated, succeeds
	Given I am logged in as Greg
	And I set organization setting public rental on
	And I am logged in as John
	When I try to add article to cart
	Then my cart should have these items:
	| Text  | Item Total |
	| Apple | 1.10       |