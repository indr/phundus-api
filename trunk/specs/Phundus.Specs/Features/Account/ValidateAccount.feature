Feature: ValidateAccount
	
Scenario: I can log in after account validation
	Given I signed up
	And I got the validation key from account validation email
	And I validated the key
	When I try to log in
	Then I should be logged in
