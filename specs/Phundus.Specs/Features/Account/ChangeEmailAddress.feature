Feature: ChangeEmailAddress
	
Background: 
	Given a confirmed user
	And logged in as user

Scenario: sends email "Validierung der geänderten E-Mail-Adresse"
	When change email address
	Then user should receive email "[phundus] Validierung der geänderten E-Mail-Adresse" at requested address
