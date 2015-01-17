Feature: ReservationSaga
	
Background:
	Given empty order created

Scenario: Order item added dispatches reserve article
	When order item added 1
	Then reserve article

Scenario: Idempotent order item added
	Given order item added 1
	When order item added 1
	Then no commands dispatched

Scenario: Order item removed dispatches cancel reservation
	Given order item added 1
	When order item removed 1
	Then cancel reservation 1

Scenario: Idempotent order item removed
	Given order item added 1
	And order item removed 1
	When order item removed 1
	Then no commands dispatched

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
	When order rejected
	Then cancel reservation 1

Scenario: Idempotent order rejected
	Given order item added 1
	And order rejected
	When order rejected
	Then no commands dispatched

Scenario: When an order is closed dispatches cancel reservation
	Given order item added 1
	When order closed
	Then cancel reservation 1	

Scenario: Idempotent order closed
	Given order item added 1
	And order closed
	When order closed
	Then no commands dispatched