Feature: QuantitiesInInventoryProjection

Background: 
	Given stock created "Stock-Id-1", article 1234, organization 1001
	And quantity in inventory increased of 2 to 2 as of 09.11.2014
	And quantity in inventory decreased of 1 to 1 as of 13.11.2014
	And quantity in inventory increased of 3 to 3 as of 08.11.2014
	And now is 11.11.2014 15:52:00

Scenario: Increase quantity in inventory in past, check for new total
	When I ask for the current quantity in inventory
	Then the result should be 5

Scenario: Get all quantity in inventory changes for article
	When I ask for all quantities in inventory
	Then the quantities in inventory should be
	| AsOfUtc             | Change | Total |
	| 08.11.2014 00:00:00 | 3      | 3     |
	| 09.11.2014 00:00:00 | 2      | 5     |
	| 13.11.2014 00:00:00 | -1     | 4     |