Feature: QuantityAvailable

Background: 
	Given stock created "Stock-Id-1", article 1234, organization 1001

Scenario: Increase quantity in inventory
	When increase quantity in inventory of 3 as of 01.01.2014
	Then quantity available changed from 01.01.2014 of 3
	And quantities available
	| AsOfUtc    | Change | Total |
	| 01.01.2014 | 3      | 3     |	

Scenario: Increase quantity in inventory at the same date 
	Given quantity in inventory increased of 10 to 10 as of 01.02.2015
	And quantity available changed from 01.02.2015 of 10
	When increase quantity in inventory of 3 as of 01.02.2015
	Then quantity available changed from 01.02.2015 of 3
	And quantities available
	| AsOfUtc    | Change  | Total  |
	| 01.02.2015 | 13      | 13     |	

Scenario: Increase quantity with future quantity
	Given quantity in inventory increased of 1 to 1 as of 01.03.2015
	And quantity available changed from 01.03.2015 of 1
	When increase quantity in inventory of 2 as of 01.02.2015
	Then quantity available changed from 01.02.2015 of 2
	And quantities available
	| AsOfUtc    | Change | Total |
	| 01.02.2015 | 2      | 2     |
	| 01.03.2015 | 1      | 3     |