Feature: ReservationSaga
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background:
	Given empty order created

Scenario: Order item added dispatches reserve article
	When order item added
	Then reserve article

Scenario: Order item added when already handled does not dispatch reserve article
	Given order item added
	When order item added
	Then no commands dispatched

Scenario: Order item removed dispatches cancel reservation
	Given order item added
	When order item removed
	Then cancel reservation

Scenario: Order item period changed dispatches change reservation period
	Given order item added
	When order item period changed
	Then change reservation period

Scenario: Order item quantity changed dispatches change reservation quantity
	Given order item added
	When order item quantity changed
	Then change reservation quantity

Scenario: When an order is rejected dispatches cancel reservation
	Given order item added
	When reject order
	Then cancel reservation

Scenario: When an order is closed dispatches cancel reservation
	Given order item added
	When close order
	Then cancel reservation	