Feature: AvailabilityDetails
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: No reservations
	Given an article with gross stock of 2
	And now is 16.08.2014 10:00:00
	When I ask for availability details
	Then the result should be
	| FromUtc			  | Quantity |
	| 15.08.2014 22:00:00 | 2        |

Scenario: One reservation in the future
	Given an article with gross stock of 1
	And now is 16.08.2014 10:00:00
	And these reservations exists
	| FromUtc             | ToUtc               | Quantity |
	| 18.08.2014 00:00:00 | 19.08.2014 23:59:59 | 1        |
	When I ask for availability details
	Then the result should be
	| FromUtc             | Quantity |
	| 15.08.2014 22:00:00 | 1        |
	| 18.08.2014 00:00:00 | 0        |
	| 20.08.2014 00:00:00 | 1        |

Scenario: One reservation in the past
	Given an article with gross stock of 2
	And now is 16.08.2014 10:00:00
	And these reservations exists
	| FromUtc             | ToUTc               | Quantity |
	| 13.08.2014 00:00:00 | 14:08:2014 23:59:59 | 2        |
	When I ask for availability details
	Then the result should be
	| FromUtc             | Quantity |
	| 15.08.2014 22:00:00 | 2        |

Scenario: One reservation today
	Given an article with gross stock of 20
	And now is 18.08.2014 10:27:00
	And these reservations exists
	| FromUtc             | ToUtc               | Quantity |
	| 17.08.2014 22:00:00 | 18.08.2014 21:59:59 | 1        |
	When I ask for availability details
	Then the result should be
	| FromUtc             | Quantity |
	| 17.08.2014 22:00:00 | 19       |
	| 18.08.2014 22:00:00 | 20       |

Scenario: Multiple reservations
	Given an article with gross stock of 5
	And now is 16.08.2014 10:00:00
	And these reservations exists
	| FromUtc             | ToUtc               | Quantity |
	| 14.08.2014 00:00:00 | 15.08.2014 23:59:59 | 1        |
	| 14.08.2014 22:00:00 | 16.08.2014 21:59:59 | 1        |
	| 17.08.2014 00:00:00 | 19.08.2014 23:59:59 | 2        |
	| 18.08.2014 00:00:00 | 20.08.2014 23:59:59 | 3        |
	When I ask for availability details
	Then the result should be
	| FromUtc             | Quantity |
	| 15.08.2014 22:00:00 | 3        |
	| 16.08.2014 00:00:00 | 4        |
	| 16.08.2014 22:00:00 | 5        |
	| 17.08.2014 00:00:00 | 3        |
	| 18.08.2014 00:00:00 | 0        |
	| 20.08.2014 00:00:00 | 2        |
	| 21.08.2014 00:00:00 | 5        |
