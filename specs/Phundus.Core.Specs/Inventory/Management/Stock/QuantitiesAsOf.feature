﻿Feature: QuantitiesAsOf
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: Change quantity at the same date time
	Given I changed quantity of 1 at 16.01.2015 10:20:30
	When I change quantity of 2 at 16.01.2015 10:20:30
	Then the total as of 16.01.2015 10:20:30 should be 3
	And the quantities should be
	| AsOfUtc             | Change | Total |
	| 16.01.2015 10:20:30 | 3      | 3     |
