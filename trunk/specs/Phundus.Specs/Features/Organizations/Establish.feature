Feature: Establish organization
	
Background: 
	Given I am logged in as root	

Scenario: Organization is in query result
	Given I established an organization
	When I try to query all organizations
	Then I should find the organization in the result

Scenario: Get organization details
	Given I established an organization
	When I try to get the organization details
	Then I should see the organization details