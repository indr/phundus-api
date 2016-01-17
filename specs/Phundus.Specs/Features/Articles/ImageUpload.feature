Feature: ImageUpload
	

Scenario: Upload to an article in user store
	Given I am logged in as a user
	And I opened my store
	And I created an article in my store
	When I try to upload an article image
	Then I should see ok

Scenario: Upload to an article in organization store
	Given an organization with these members
	| Alias | Role    |
	| Greg  | Manager |
	And I am logged in as Greg
	And I created an article in the default store
	When I try to upload an article image
	Then I should see ok