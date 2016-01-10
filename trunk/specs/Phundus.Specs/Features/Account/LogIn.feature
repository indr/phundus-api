﻿Feature: LogIn

Scenario: Log in successful
	Given a confirmed user
	When log in
	Then logged in

Scenario: Log in failed cause not confirmed
	Given a user
	When log in
	Then not logged in

Scenario: Log in failed cause of lock
	Given a confirmed, locked user	
	When log in
	Then not logged in

Scenario: Log in successful after unlock
	Given a confirmed, locked user
	And unlock user
	When log in
	Then logged in

