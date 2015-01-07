Feature: Allocation
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
	Given stock created "Stock-1", article 10001, organization 1001
	And quantity in inventory increased of 10 to 10 as of 07.01.2015

Scenario: Allocation with sufficient quantity in inventory
	When allocate 1 with id 1 for reservation 1 from 07.01.2015 to 08.01.2015
	Then stock allocated 1, Promised

Scenario: Allocation with insufficient quantity in inventory
	When allocate 11 with id 2 for reservation 2 from 07.01.2015 to 08.01.2015
	Then stock allocated 2, Impossible
