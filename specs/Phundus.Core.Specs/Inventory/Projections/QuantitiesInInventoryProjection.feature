Feature: QuantitiesInInventoryProjection
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
	Given stock created "Stock-Id-1"
	And now is 11.11.2014 15:52:00

Scenario: Add two numbers
	Given quantity in inventory increased of 2 to 2 as of 09.11.2014
	And quantity in inventory increased of 3 to 3 as of 08.11.2014
	When I ask for the current quantity in inventory
	Then the result should be 5
