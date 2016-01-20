Feature: MaintenanceModeRestrictions
	
Background:
	Given in maintenance mode

@inMaintenance
Scenario: Log in with external email address fails with service unavailable
	When I try to log in with "user@domain.com"
	Then I should see service unavailable

@inMaintenance
Scenario: Log in with @phundus.ch fails with invalid password 
	When I try to log in with "user@phundus.ch"
	Then I should see error

@inMaintenance
Scenario: Log in with @test.phundus.ch fails with invalid password
	When I try to log in with "user@test.phundus.ch"
	Then I should see error

@inMaintenance
Scenario: Sign up with external email address fails with service unavailable
	When I try to sign up with "user@domain.com"
	Then I should see service unavailable

@inMaintenance
Scenario: Sign up with @test.phundus.ch succeeds
	When I try to sign up
	Then I should see ok