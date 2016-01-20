Feature: Maintenance mode activation
	

Scenario: Activate maintenance mode as user fails
	Given I am logged in as a user
	When I try to activate maintenance mode
	Then I should see unauthorized

Scenario: Activate maintenance mode, success
	Given I am logged in as root
	When I try to activate maintenance mode
	Then I should see server status in maintenance true

Scenario: Deactivate maintenance mode, success
	Given I am logged in as root
	And I activated maintenance mode
	When I try to deactivate maintenance mode
	Then I should see server status in maintenance false