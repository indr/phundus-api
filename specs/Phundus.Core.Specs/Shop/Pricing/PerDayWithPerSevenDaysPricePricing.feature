Feature: PerDayWithPerSevenDaysPricePricing

Scenario: Same day
	Given a per week price of 14.00 CHF
	When I calculate the per day price with these values
	| FromLocal           | ToLocal             | Amount |
	| 19.08.2014 22:00:00 | 20.08.2014 21:59:59 | 1      |
	| 20.08.2014 10:00:00 | 20.08.2014 14:00:00 | 2      |
	| 20.08.2014 00:00:00 | 20.08.2014 23:59:59 | 3      |
	Then the resulting prices should be
	| Days | Price |
	| 2	   | 4.00  |
	| 1    | 4.00  |
	| 1    | 6.00  |

Scenario: Up to seven days
	Given a per week price of 7.00 CHF
	When I calculate the per day price with these values
	| FromLocal           | ToLocal             | Amount |
	| 20.08.2014 20:00:00 | 21.08.2014 02:00:00 | 1      |
	| 20.08.2014 20:00:00 | 23.08.2014 23:59:59 | 2      |
	| 20.08.2014 23:59:59 | 26.08.2014 00:00:00 | 3      |
	Then the resulting prices should be
	| Days | Price |
	| 2    | 2.00  |
	| 4    | 8.00  |
	| 7    | 21.00 |


Scenario: Seven days or more
	Given a per week price of 7.00 CHF
	When I calculate the per day price with these values
	| FromLocal           | ToLocal             | Amount |
	| 20.08.2014 00:00:00 | 27.08.2014 00:00:00 | 1      |
	| 20.08.2014 20:00:00 | 28.08.2014 23:59:59 | 2      |
	| 20.08.2014 20:00:00 | 29.08.2014 23:59:59 | 3      |
	Then the resulting prices should be
	| Days | Price |
	| 8    | 8.00  |
	| 9    | 18.00 |
	| 10   | 30.00 |


Scenario: Round to closest integer
	Given a per week price of 2.31 CHF
	When I calculate the per day price with these values
    | FromLocal           | ToLocal             | Amount |
    | 14.12.2014 00:00:00 | 14.12.2014 23:59:59 | 1      |
    | 14.12.2014 00:00:00 | 15.12.2014 23:59:59 | 1      |
    | 14.12.2014 00:00:00 | 16.12.2014 23:59:59 | 1      |
	| 14.12.2014 00:00:00 | 17.12.2014 23:59:59 | 1      |
	| 14.12.2014 00:00:00 | 18.12.2014 23:59:59 | 1      |
	Then the resulting prices should be
    | Days | Price |
    | 1    | 1.00  |
    | 2    | 1.00  |
    | 3    | 1.00  |
    | 4    | 1.00  |
    | 5    | 2.00  |