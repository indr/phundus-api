Feature: Availability
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: No reservations
	Given an article with gross stock of 2
	And now is 16.08.2014 10:00:00
	When I ask for availability
	Then the result should be
	| FromUtc			  | Amount |
	| 16.08.2014 00:00:00 | 2      |

Scenario: Reservations in the future
	Given an article with gross stock of 5
	And now is 16.08.2014 10:00:00
	And these reservations exists
	| FromUtc    | ToUtc      | Amount |
	| 17.08.2014 | 19.08.2014 | 2      |
	| 18.08.2014 | 20.08.2014 | 3      |
	When I ask for availability
	Then the result should be
	| FromUtc    | Amount |
	| 16.08.2014 | 5      |
	| 17.08.2014 | 3      |
	| 18.08.2014 | 0      |
	| 20.08.2014 | 2      |
	| 21.08.2014 | 5      |