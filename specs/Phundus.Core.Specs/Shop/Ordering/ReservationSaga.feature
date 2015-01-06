Feature: ReservationSaga
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background:
	Given empty order created

Scenario: Order item added dispatches reserve article
	When order item added 1
	Then reserve article

Scenario: Order item added when already handled does not dispatch reserve article
	Given order item added 1	
	When order item added 1
	Then no commands dispatched

Scenario: Order item removed dispatches cancel reservation
	Given order item added 1
	When order item removed 1
	Then cancel reservation 1

Scenario: Order item period changed dispatches change reservation period
	Given order item added 1
	When order item period changed 1
	Then change reservation period 1

Scenario: Order item quantity changed dispatches change reservation quantity
	Given order item added 1
	When order item quantity changed 1
	Then change reservation quantity 1

Scenario: When an order is rejected dispatches cancel reservation
	Given order item added 1
	And order item added 2
	When reject order
	Then cancel reservation 1

Scenario: When an order is closed dispatches cancel reservation
	Given order item added 1
	And order item added 2
	When close order
	Then cancel reservation 1	