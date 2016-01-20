Feature: MaintenanceModeRestrictions
	
Background:
	Given in maintenance mode

@inMaintenance
Scenario: Log in with external email address fails with service unavailable
	When I try to log in with "user@domain.com"
	Then I should see service unavailable
	And I should see message "Das System befindet sich im Wartungsmodus."

@inMaintenance
Scenario: Log in with @phundus.ch fails with not found 
	When I try to log in with "not-found@phundus.ch"
	Then I should see not found

@inMaintenance
Scenario: Log in with @test.phundus.ch fails with not found
	When I try to log in with "not-found@test.phundus.ch"
	Then I should see not found

@inMaintenance
Scenario: Sign up with external email address fails with service unavailable
	When I try to sign up with "user@domain.com"
	Then I should see service unavailable

@inMaintenance
Scenario: Sign up with @test.phundus.ch succeeds
	When I try to sign up
	Then I should see ok

Scenario: Emails aren't sent to external domains
	When I try to submit feedback with email address "john@example.com"
	Then "john@example.com" should not receive email "Vielen Dank fürs Feedback"
	And "admin@test.phundus.ch" should receive email "[phundus] Feedback"