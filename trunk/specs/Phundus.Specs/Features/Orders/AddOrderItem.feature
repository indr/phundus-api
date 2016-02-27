﻿Feature: AddOrderItem

Background: 
	Given an organization "Scouts" with these members
    | Alias | Role    |
	| Greg  | Manager |
	| Alice | Member  |
	And with these organization articles
	| Alias | Name  | Member price | Public price |
	| Apple | Apple | 7.00         | 14.00        |
	And a confirmed user "John" with email address "john@test.phundus.ch"
	And I am logged in as Greg
	

Scenario: Add article to order for member picks member price
	Given I created a new empty order for Alice
	When I try to add the article Apple to the order
	Then the order should have these items:
	| Text  | Item Total |
	| Apple | 1.00       |

Scenario: Add article to order for non member picks public price
	Given I created a new empty order for John
	When I try to add the article Apple to the order
	Then the order should have these items:
	| Text  | Item Total |
	| Apple | 2.00       |