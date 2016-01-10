Feature: ChangeEmailAddress
	
Background: 
	Given a confirmed user	
	And logged in as user

@ignore
Scenario: sends email "Validierung der geänderten E-Mail-Adresse"
	When change email address
	Then user should receive email "[phundus] Validierung der geänderten E-Mail-Adresse" at requested address

@ignore
Scenario: can not login without validation
	Given user changed email address
	When log in with requested address
	Then not logged in

Scenario: can login with new address after validation
	Given user changed email address
	And the validation key from email validation email
	When validate key
	And log in with requested address
	Then logged in

Scenario: can not change email if already taken
	Given a confirmed user "Johan"
	And a confirmed user "John" with email address "john@test.phundus.ch"	
	And logged in as "Johan"
	When "Johan" changes email address to "john@test.phundus.ch"
	Then error email address already taken

Scenario: can not validate if already taken
	Given a confirmed and logged in user "Johan"
	And "Johan" changed email address to "john@test.phundus.ch"
	And the validation key from email validation email
	And a confirmed user "John" with email address "john@test.phundus.ch"
	When validate key
	Then not validated
	