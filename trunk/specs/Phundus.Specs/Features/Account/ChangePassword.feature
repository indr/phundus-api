Feature: ChangePassword
	
Background: 
	Given I am logged in as a user

Scenario: can log in with new password
	Given I changed my password
	When  I try to login with my new password
	Then I should be logged in
