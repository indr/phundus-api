Feature: ValidateAccount
	
Scenario: Validate key
	Given a user
	And the validation key from account validation email
	When validate account
	Then can log in
