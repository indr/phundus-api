Feature: ResetPassword

Background: 
	Given a confirmed user
	And logged in as user

Scenario: sends email "Neues Passwort"
	When reset password
	Then user should receive email "[phundus] Neues Passwort"
