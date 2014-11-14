Feature: QuantityInInventory
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
	Given stock created "Stock-Id-1", article 1234, organization 1001

Scenario: Increment quantity the first time
	When Increase quantity in inventory of 3 as of 01.01.2013
	Then quantity in inventory increased of 3 to 3 as of 01.01.2013

Scenario: Increment quantity in inventory
	Given quantity in inventory increased of 2 to 2 as of 09.11.2014
	And quantity in inventory increased of 3 to 3 as of 08.11.2014
	When Increase quantity in inventory of 1 as of 11.11.2014
	Then quantity in inventory increased of 1 to 6 as of 11.11.2014

Scenario: Decrement quantity in inventory
	Given quantity in inventory increased of 10 to 10 as of 09.11.2014
	When Decrease quantity in inventory of 2 as of 10.11.2014
	Then quantity in inventory decreased of 2 to 8 as of 10.11.2014
