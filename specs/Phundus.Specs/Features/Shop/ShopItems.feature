Feature: ShopItems
	

Background:
	Given an organization with these members
	| Alias | Role    |
	| Greg  | Manager |
	And I am logged in as Greg


Scenario: View shop item details
	Given I created an article in the default store with these values
	| Alias | Name   | Gross stock | Member price | Public price |
	| Apple | Boskop | 3           | 1.50         | 1.60         |
	And I uploaded an article image image1.jpg
	And I uploaded an article document doc1.pdf
	When I try to get the shop item details
	Then the shop item should equal
	| Name   | Member price | Public price |
	| Boskop | 1.50         | 1.60         |
	And the shop item should have 1 document
	And the shop item should have 1 image
