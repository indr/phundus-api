Feature: ImageUpload
	

Scenario: Upload to an article in user store
	Given I am logged in as a user
	And I opened my store
	And I created an article in my store
	When I try to upload an article image Image1.jpg
	Then I should get file upload response content
	| Name       | Type      |
	| Image1.jpg | image/jpg |

Scenario: Upload to an article in organization store
	Given an organization with these members
	| Alias | Role    |
	| Greg  | Manager |
	And I am logged in as Greg
	And I created an article in the default store
	When I try to upload an article image Image2.jpg
	Then I should get file upload response content
	| Name       | Type      |
	| Image2.jpg | image/jpg |