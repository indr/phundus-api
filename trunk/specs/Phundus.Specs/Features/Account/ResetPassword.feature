Feature: ResetPassword

Background: 
	Given a confirmed user
	And logged in

Scenario: sends email "Neues Passwort"
	When reset password
	Then user should receive email "[phundus] Neues Passwort"
