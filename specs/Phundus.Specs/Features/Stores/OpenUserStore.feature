Feature: OpenUserStore

Scenario: Open user store, user has store
	Given a confirmed user
	And logged in
	When open user store
	Then get user with store

Scenario: Open a second store returns 500
	Given a confirmed user
	And logged in
	When open user store
	And open user store
	Then response code is 500
