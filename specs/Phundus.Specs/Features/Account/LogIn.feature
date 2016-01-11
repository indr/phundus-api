Feature: LogIn

Scenario: Log in successful
	Given I signed up and confirmed my email address
	When I try to log in
	Then I should be logged in

Scenario: Log in failed cause not confirmed
	Given I signed up
	When I try to log in
	Then I should not be logged in

Scenario: Log in failed cause of lock
	Given I signed up and confirmed my email address
	And an administrator locked my account
	When I try to log in
	Then I should not be logged in

Scenario: Log in successful after unlock
	Given I signed up and confirmed my email address
	And an administrator locked my account
	And an administrator unlocked my account
	When I try to log in
	Then I should be logged in

