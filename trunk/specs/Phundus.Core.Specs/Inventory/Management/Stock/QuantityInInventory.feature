Feature: QuantityInInventory

Background: 
	Given stock created "Stock-Id-1", article 1234, organization 1001

Scenario: Increment quantity without any quantities in past or future
	When increase quantity in inventory of 3 as of 01.01.2013
	Then quantity in inventory increased of 3 to 3 as of 01.01.2013
	And quantities in inventory
	| AsOfUtc             | Change | Total |
	| 01.01.2013 00:00:00 | 3      | 3     |
	
Scenario: Increment quantity in inventory with past quantities
	Given quantity in inventory increased of 2 to 2 as of 09.11.2014
	And quantity in inventory increased of 3 to 3 as of 08.11.2014
	When increase quantity in inventory of 1 as of 11.11.2014
	Then quantity in inventory increased of 1 to 6 as of 11.11.2014	
	And quantities in inventory
	| AsOfUtc             | Change | Total |
	| 08.11.2014 00:00:00 | 3      | 3     |
	| 09.11.2014 00:00:00 | 2      | 5     |
	| 11.11.2014 00:00:00 | 1      | 6     |	


Scenario: Increment quantity in inventory with future quantity
	Given quantity in inventory increased of 1 to 1 as of 09.11.2014
	When increase quantity in inventory of 2 as of 08.11.2014
	Then quantity in inventory increased of 2 to 2 as of 08.11.2014
	And quantities in inventory
	| AsOfUtc    | Change | Total |
	| 08.11.2014 | 2      | 2     |
	| 09.11.2014 | 1      | 3     |

Scenario: Decrement quantity in inventory with past quantity
	Given quantity in inventory increased of 10 to 10 as of 09.11.2014
	When decrease quantity in inventory of 2 as of 10.11.2014
	Then quantity in inventory decreased of 2 to 8 as of 10.11.2014	
	And quantities in inventory
	| AsOfUtc    | Change | Total |
	| 09.11.2014 | 10     | 10    |
	| 10.11.2014 | -2     | 8     |	
