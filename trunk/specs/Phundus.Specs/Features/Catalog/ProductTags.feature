Feature: ProductTags
	
	
Scenario: Tag product
	Given an organization with these members
	| Alias | Role    |
	| Greg  | Manager |
	And I am logged in as Greg
	And I created an article in the default store
	When I tag a product with "tag1"
	Then the tag "tag1" should be in the public list