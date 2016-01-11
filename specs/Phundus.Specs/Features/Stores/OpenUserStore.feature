Feature: OpenUserStore

Background:
	Given I am logged in as a user


Scenario: User opens his store
	When I try to open my store
	Then I should see ok

Scenario: User opens a second store, and fails
	Given I opened my store
	When I try to open my store
	Then I should see error

Scenario: User gets his store info
	Given I opened my store
	When I try to get my user details
	Then I should see the store
