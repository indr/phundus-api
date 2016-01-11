Feature: ResetPassword

Background: 
	Given a confirmed user

Scenario: sends email "Neues Passwort"
	When I try to reset user's password
	Then user should receive email "[phundus] Neues Passwort"
