Feature: ChangeEmailAddress

Scenario: sends email "Validierung der geänderten E-Mail-Adresse"
	Given I am logged in as a user
	When I try to change my email address
	Then I should receive an email "[phundus] Validierung der geänderten E-Mail-Adresse" at requested address

Scenario: can not login without validation
	Given I am logged in as a user
	And I changed my email address
	When I try to log in with requested address
	Then I should not be logged in

Scenario: can login with new address after validation
	Given I am logged in as a user
	And I changed my email address
	And I got the validation key from email validation email
	And I validated the key
	When I try to log in with requested address	
	Then I should be logged in

Scenario: can not change email if already taken	
	Given a confirmed user "Johan"
	And a confirmed user "John" with email address "john@test.phundus.ch"	
	And I am logged in as "Johan"
	When I try to change my email address to "john@test.phundus.ch"
	Then error email address already taken

Scenario: can not validate if already taken
	Given a confirmed user "Johan"
	And I am logged in as "Johan"
	And I changed email address to "john@test.phundus.ch"
	And I got the validation key from email validation email
	And a confirmed user "John" with email address "john@test.phundus.ch"
	And I am logged in as "Johan"
	When I try to validate the key
	Then I should see error
	