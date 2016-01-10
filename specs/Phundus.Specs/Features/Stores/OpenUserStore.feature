Feature: OpenUserStore
	

@mytag
Scenario: Open user store, user has store
	Given a confirmed user
	And logged in
	When open user store
	Then get user with store
