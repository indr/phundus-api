Feature: ChangePassword
	
Background: 
	Given a confirmed user
	And logged in

Scenario: can log in with new password
	Given change password
	When log in
	Then logged in
