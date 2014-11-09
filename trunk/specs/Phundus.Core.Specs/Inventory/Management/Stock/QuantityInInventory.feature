Feature: QuantityInInventory
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
	Given stock created

Scenario: Increment quantity in inventory
	When Increase quantity in inventory 1
	Then quantity in inventory increased 1

Scenario: Decrement quantity in inventory
	Given quantity in inventory increased 2
	When I remove 1 from the inventory
	Then quantity in inventory decreased 1
