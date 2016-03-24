Feature: IsAvailable
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers


Scenario: No reservations available sufficient quantity
	Given an article with gross stock of 2
	And now is 18.08.2014 06:36:00
	When I ask for availability from 17.08.2014 22:00:00 to 18.08.2014 21:59:59 of 2
	Then the result should be true

Scenario: No reservations available insufficient quantity
	Given an article with gross stock of 2
	And now is 16.08.2014 10:00:00
	When I ask for availability from 18.08.2014 00:00:00 to 20.08.2014 23:59:59 of 3
	Then the result should be false

Scenario: One reservation in the future insufficient quantity
	Given an article with gross stock of 2
	And now is 16.08.2014 10:00:00
	And these reservations exists
	| FromUtc             | ToUtc               | Quantity |
	| 18.08.2014 00:00:00 | 19.08.2014 23:59:59 | 1        |
	When I ask for availability from 18.08.2014 00:00:00 to 20.08.2014 23:59:59 of 3
	Then the result should be false

Scenario: One reservation in the future sufficient quantity
	Given an article with gross stock of 2
	And now is 16.08.2014 10:00:00
	And these reservations exists
	| FromUtc             | ToUtc               | Quantity |
	| 18.08.2014 00:00:00 | 19.08.2014 23:59:59 | 1        |
	When I ask for availability from 18.08.2014 00:00:00 to 20.08.2014 23:59:59 of 1
	Then the result should be true

Scenario: One reservation in the past
	Given an article with gross stock of 2
	And now is 16.08.2014 10:00:00
	And these reservations exists
	| FromUtc             | ToUTc               | Quantity |
	| 13.08.2014 00:00:00 | 14:08:2014 23:59:59 | 2      |
	When I ask for availability from 18.08.2014 00:00:00 to 20.08.2014 23:59:59 of 2
	Then the result should be true

Scenario: Multiple reservations insufficient quantity
	Given an article with gross stock of 5
	And now is 16.08.2014 10:00:00
	And these reservations exists
	| FromUtc             | ToUtc               | Quantity |
	| 14.08.2014 00:00:00 | 15.08.2014 23:59:59 | 1        |
	| 14.08.2014 22:00:00 | 16.08.2014 21:59:59 | 1        |
	| 17.08.2014 00:00:00 | 19.08.2014 23:59:59 | 2        |
	| 18.08.2014 00:00:00 | 20.08.2014 23:59:59 | 2        |
	When I ask for availability from 18.08.2014 00:00:00 to 20.08.2014 23:59:59 of 2
	Then the result should be false

Scenario: Multiple reservations sufficient quantity
	Given an article with gross stock of 5
	And now is 16.08.2014 10:00:00
	And these reservations exists
	| FromUtc             | ToUtc               | Quantity |
	| 14.08.2014 00:00:00 | 15.08.2014 23:59:59 | 1        |
	| 14.08.2014 22:00:00 | 16.08.2014 21:59:59 | 1        |
	| 17.08.2014 00:00:00 | 19.08.2014 23:59:59 | 2        |
	| 18.08.2014 00:00:00 | 20.08.2014 23:59:59 | 2        |
	When I ask for availability from 18.08.2014 00:00:00 to 20.08.2014 23:59:59 of 1
	Then the result should be true

Scenario: Multiple questions
	Given an article with gross stock of 1
	And now is 16.08.2014 10:00:00
	When I ask for multiple availability
	| FromUtc             | ToUtc               | Quantity |
	| 18.08.2014 22:00:00 | 21.08.2014 21:59:59 | 1        |
	| 20.08.2014 00:00:00 | 23.08.2014 23:59:59 | 1        |
	Then the result should be false