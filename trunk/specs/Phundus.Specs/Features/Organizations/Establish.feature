Feature: Establish organization
	
Scenario: Organization is in query result
	Given I am logged in as root	
	And I established an organization
	When I try to query all organizations
	Then I should find the organization in the result

Scenario: Get organization details
	Given I am logged in as root
	And I established an organization
	When I try to get the organization details
	Then I should see the organization details

Scenario: Establish organization as user, fails
	Given I am logged in as a user
	When I try to establish an organization
	Then I should see unauthorized