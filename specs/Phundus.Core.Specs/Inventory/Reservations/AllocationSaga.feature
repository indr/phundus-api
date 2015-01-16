Feature: AllocationSaga

Scenario: Article reserved dispatches allocate stock
	When article reserved
	Then allocation saga state is Allocated
	Then allocate stock

Scenario: Idempotent article reserved
	Given article reserved
	When article reserved
	Then no commands dispatched

Scenario: Reservation cancelled dispatches remove allocation
	Given article reserved
	When reservation cancelled
	Then discard allocation

Scenario: Idempotent reservation cancelled
	Given article reserved
	And reservation cancelled
	When reservation cancelled
	Then no commands dispatched
	
Scenario: Reservation period changed dispatches change allocation period
	Given article reserved
	When reservation period changed
	Then change allocation period

Scenario: Reservation quantity changed dispatches change allocation quantity
	Given article reserved
	When reservation quantity changed
	Then change allocation quantity
