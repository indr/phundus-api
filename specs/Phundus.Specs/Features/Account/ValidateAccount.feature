Feature: ValidateAccount
	
Scenario: Validate key
	Given a user
	And the validation key from account validation email
	When validate key
	Then can log in
