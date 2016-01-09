Feature: ChangeEmailAddress
	
Background: 
	Given a confirmed user	
	And logged in as user
	

Scenario: sends email "Validierung der geänderten E-Mail-Adresse"
	When change email address
	Then user should receive email "[phundus] Validierung der geänderten E-Mail-Adresse" at requested address

Scenario: can not login without validation
	Given user changed email address
	When log in with requested address
	Then not logged in

Scenario: can login after validation
	Given user changed email address
	And the validation key from email validation email
	When validate key
	And log in with requested address
	Then logged in
