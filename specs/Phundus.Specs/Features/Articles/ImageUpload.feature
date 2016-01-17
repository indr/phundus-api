Feature: ImageUpload
	
Background: 
	Given I am logged in as a user
	And I opened my store
	And I created an article in my store


Scenario: Image upload succeeds
	When I try to upload an article image
	Then I should see ok
