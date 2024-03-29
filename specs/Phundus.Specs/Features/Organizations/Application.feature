﻿Feature: Application
	
Scenario: Relationship status after application
	Given an organization
	And I am logged in as a user
	And I applied for membership	
	Then my relationship status is "application"

Scenario: Relationship status after rejection
	Given an organization
	And I am logged in as "John"
	And I applied for membership
	And the membership application is rejected
	When I am logged in as "John"	
	Then my relationship status is "rejected"

Scenario: Relationship status is after approval
	Given an organization
	And I am logged in as "Greg"
	And I applied for membership
	And the membership application is approved
	And I am logged in as "Greg"	
	Then my relationship status is "member"

Scenario: New member is in the organizations member list
	Given an organization
	And I am logged in as "Greg"
	And I applied for membership
	And the membership application is approved
	And I am logged in as root
	When I try to query all organization members
	Then I should find "Greg" in members

Scenario: A second application request fails
	Given an organization
	And I am logged in as "Greg"
	And I applied for membership
	When I try to apply for membership
	Then I should see error