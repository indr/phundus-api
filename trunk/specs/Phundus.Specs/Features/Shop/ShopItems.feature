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
	And I updated the article description:
	"""
	This is the multiline
	article description
	"""
	And I updated the article specification:
	"""
	This is the multiline
	article specification
	"""
	And I uploaded an article image image1.jpg
	And I uploaded an article image image2.jpg
	And I uploaded an article image image3.jpg
	And I uploaded an article image image4.jpg
	And I uploaded an article image image5.jpg
	And I uploaded an article document doc1.pdf
	And I uploaded an article document doc2.pdf
	And I am logged out	
	Then the shop item should equal
	| Name   | Member price | Public price |
	| Boskop | 1.50         | 1.60         |
	And the shop item should have 2 document
	And the shop item should have 5 image
